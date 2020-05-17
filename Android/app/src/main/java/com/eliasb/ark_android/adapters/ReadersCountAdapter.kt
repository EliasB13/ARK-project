package com.eliasb.ark_android.adapters

import android.annotation.SuppressLint
import android.content.Context
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import android.widget.Toast
import androidx.core.content.ContextCompat
import androidx.navigation.Navigation
import androidx.recyclerview.widget.RecyclerView
import com.eliasb.ark_android.R
import com.eliasb.ark_android.model.dtos.ReaderCountStat
import com.eliasb.ark_android.services.PreferencesService
import com.eliasb.ark_android.services.StatisticService
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.schedulers.Schedulers
import kotlinx.android.synthetic.main.common_stat_item.view.*
import org.threeten.bp.LocalDateTime

class ReadersCountAdapter(val context: Context) : RecyclerView.Adapter<ReadersCountAdapter.ReadersViewHolder>() {
    private val client by lazy { StatisticService.create() }
    private val readers: ArrayList<ReaderCountStat> = ArrayList()

    class ReadersViewHolder(val view: View) : RecyclerView.ViewHolder(view)

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ReadersViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.common_stat_item, parent, false)

        return ReadersViewHolder(view)
    }

    override fun onBindViewHolder(holder: ReadersViewHolder, position: Int) {

        holder.view.name.text = readers[position].name
        holder.view.description.text = readers[position].description
        if (readers[position].isEntrance) {
            holder.view.isEntrance.text = context.getString(R.string.yes)
            holder.view.isEntrance.setTextColor(ContextCompat.getColor(context, R.color.success))
        } else {
            holder.view.isEntrance.text = context.getString(R.string.no)
            holder.view.isEntrance.setTextColor(ContextCompat.getColor(context, R.color.danger))
        }

        holder.view.emplCount.text = readers[position].employeesObservationsCount.toString()
        holder.view.anonCount.text = readers[position].anonymObservationsCount.toString()

        val bundle = Bundle()
        bundle.putInt(context.getString(R.string.reader_id), readers[position].id)
        holder.view.setOnClickListener(Navigation.createNavigateOnClickListener(R.id.action_nav_statistic_to_readerStatisticFragment, bundle))
    }

    override fun getItemCount(): Int = readers.size

    @SuppressLint("CheckResult")
    fun refreshReaders(emptyListView: TextView, itemsList: RecyclerView, spinner: View) {
        val prefService = PreferencesService
        prefService.create(context, context.getString(R.string.user_pref))
        val token = prefService.getPreference(context.getString(R.string.token))
        val currDate = LocalDateTime.now()
        val monthAgo = currDate.minusDays(31)

        spinner.visibility = View.VISIBLE
        client.getFullCountStatistic("Bearer $token", monthAgo.toString(), currDate.toString())
            .subscribeOn(Schedulers.io())
            .observeOn(AndroidSchedulers.mainThread())
            .subscribe({
                result -> Log.i("Res", result.toString())
                if (result.isEmpty()) {
                    emptyListView.visibility = View.VISIBLE
                    itemsList.visibility = View.GONE
                } else {
                    emptyListView.visibility = View.GONE
                    itemsList.visibility = View.VISIBLE
                }
                readers.clear()
                readers.addAll(result)
                notifyDataSetChanged()
                spinner.visibility = View.GONE
            },
            {
                error ->
                Toast.makeText(context, "Refresh error: ${error.message}", Toast.LENGTH_LONG).show()
                spinner.visibility = View.GONE
                Log.e("ERRORS", error.message)
            })
    }
}