package com.eliasb.ark_android.adapters

import android.annotation.SuppressLint
import android.content.Context
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import android.widget.Toast
import androidx.core.content.ContextCompat
import androidx.recyclerview.widget.RecyclerView
import com.eliasb.ark_android.R
import com.eliasb.ark_android.config.ApiConfig
import com.eliasb.ark_android.model.dtos.ObservationResponse
import com.eliasb.ark_android.services.PreferencesService
import com.eliasb.ark_android.services.StatisticService
import com.squareup.picasso.Picasso
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.schedulers.Schedulers
import kotlinx.android.synthetic.main.restricted_observations_item.view.*
import org.threeten.bp.LocalDateTime

private const val DEFAULT_EMPL_PHOTO = "resources/profilepics/default-empl-avatar.png"
private const val DEFAULT_ANON_PHOTO = "resources/profilepics/default-anon-avatar.png"

class RestrictedObservationsAdapter(val context: Context) : RecyclerView.Adapter<RestrictedObservationsAdapter.RestrictedObservationViewHolder>() {
    private val client by lazy { StatisticService.create() }
    private val observations: ArrayList<ObservationResponse> = ArrayList()

    class RestrictedObservationViewHolder(val view: View) : RecyclerView.ViewHolder(view) {
        private val photo: ImageView = view.findViewById(R.id.photo)

        fun updateWithUrl(url: String) {
            Picasso.get().load(url).into(photo)
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RestrictedObservationViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.restricted_observations_item, parent, false)

        return RestrictedObservationViewHolder(view)
    }

    override fun onBindViewHolder(holder: RestrictedObservationViewHolder, position: Int) {

        val personName = "${observations[position].person.name} ${observations[position].person.surname}"
        holder.view.name.text = personName

        holder.view.time.text = observations[position].time
        if (observations[position].isRestricted) {
            holder.view.item_bg.setBackgroundColor(ContextCompat.getColor(context, R.color.restrictedObservationColor))
        } else {
            holder.view.item_bg.setBackgroundColor(ContextCompat.getColor(context, R.color.white))
        }

        val imageUrl: String
        if (observations[position].person.isEmployee) {
            holder.view.isEmployee.text = context.getString(R.string.yes)
            imageUrl = "${ApiConfig.getBaseUrl()}$DEFAULT_EMPL_PHOTO"
        }
        else {
            holder.view.isEmployee.text = context.getString(R.string.no)
            imageUrl = "${ApiConfig.getBaseUrl()}$DEFAULT_ANON_PHOTO"
        }

        holder.updateWithUrl(imageUrl)
    }

    override fun getItemCount(): Int = observations.size

    @SuppressLint("CheckResult")
    fun refreshRestrictedObservations(emptyListView: TextView, itemsList: RecyclerView, spinner: View) {
        val prefService = PreferencesService
        prefService.create(context, context.getString(R.string.user_pref))
        val token = prefService.getPreference(context.getString(R.string.token))
        val currDate = LocalDateTime.now()
        val monthAgo = currDate.minusHours(24)

        spinner.visibility = View.VISIBLE
        client.getFullStatistic("Bearer $token", monthAgo.toString(), currDate.toString())
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
                observations.clear()
                result.forEach {
                    val restrictedObs = it.observations.filter { o -> o.isRestricted }
                    observations.addAll(restrictedObs)
                }
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