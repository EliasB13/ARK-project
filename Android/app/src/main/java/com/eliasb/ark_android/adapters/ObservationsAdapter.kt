package com.eliasb.ark_android.adapters

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import androidx.core.content.ContextCompat
import androidx.recyclerview.widget.RecyclerView
import com.eliasb.ark_android.R
import com.eliasb.ark_android.config.ApiConfig
import com.eliasb.ark_android.model.dtos.ObservationResponse
import com.eliasb.ark_android.services.StatisticService
import com.squareup.picasso.Picasso
import kotlinx.android.synthetic.main.common_stat_item.view.name
import kotlinx.android.synthetic.main.reader_observation_item.view.*

private const val DEFAULT_EMPL_PHOTO = "resources/profilepics/default-empl-avatar.png"
private const val DEFAULT_ANON_PHOTO = "resources/profilepics/default-anon-avatar.png"

class ObservationsAdapter(val context: Context) : RecyclerView.Adapter<ObservationsAdapter.ObservationViewHolder>() {
    private val client by lazy { StatisticService.create() }
    private val observations: ArrayList<ObservationResponse> = ArrayList()

    class ObservationViewHolder(val view: View) : RecyclerView.ViewHolder(view) {
        private val photo: ImageView = view.findViewById(R.id.photo)

        fun updateWithUrl(url: String) {
            Picasso.get().load(url).into(photo)
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ObservationViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.reader_observation_item, parent, false)

        return ObservationViewHolder(view)
    }

    override fun onBindViewHolder(holder: ObservationViewHolder, position: Int) {

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

    fun loadObservations(emptyListView: TextView, observations: ArrayList<ObservationResponse>, itemsList: RecyclerView) {
        if (observations.isEmpty()) {
            emptyListView.visibility = View.VISIBLE
            itemsList.visibility = View.GONE
        } else {
            emptyListView.visibility = View.GONE
            itemsList.visibility = View.VISIBLE
        }
        this.observations.clear()
        this.observations.addAll(observations)
        notifyDataSetChanged()
    }
}