package com.eliasb.ark_android.ui.statistic


import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.recyclerview.widget.LinearLayoutManager

import com.eliasb.ark_android.R
import com.eliasb.ark_android.adapters.ObservationsAdapter
import com.eliasb.ark_android.model.dtos.ObservationResponse
import com.eliasb.ark_android.model.dtos.ReaderStatResponse
import com.eliasb.ark_android.services.PreferencesService
import com.eliasb.ark_android.services.StatisticService
import com.squareup.picasso.Picasso
import io.reactivex.android.schedulers.AndroidSchedulers
import io.reactivex.schedulers.Schedulers
import kotlinx.android.synthetic.main.fragment_reader_statstic.*
import org.threeten.bp.LocalDateTime

class ReaderStatisticFragment : Fragment() {

    private var readerId: Int? = null
    private lateinit var adapter: ObservationsAdapter
    private var observations: ArrayList<ObservationResponse> = ArrayList()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {
            readerId = it.getInt(context!!.getString(R.string.reader_id))
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_reader_statstic, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)

        loadReaderStatistic(readerId!!)

        adapter = ObservationsAdapter(context!!)

        observationsList.layoutManager = LinearLayoutManager(context)
        observationsList.adapter = adapter
    }

    private fun loadReaderStatistic(readerId: Int) {
        val prefService = PreferencesService
        prefService.create(context!!, context!!.getString(R.string.user_pref))
        val token = prefService.getPreference(context!!.getString(R.string.token))
        if (token != null) {
            val client = StatisticService.create()
            val currDate = LocalDateTime.now()
            val monthAgo = currDate.minusDays(31)

            spinner.visibility = View.VISIBLE
            client.getReaderStatistic("Bearer $token", readerId, monthAgo.toString(), currDate.toString())
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(
                    {
                        result ->
                        Log.i("RefreshServiceItems res", result.toString())
                        result.body()?.let {
                            updateReader(it)
                            observations = it.observations
                            adapter.loadObservations(empty_list, observations, observationsList)
                        }
                        result.errorBody()?.let {
                            Toast.makeText(context!!, getString(R.string.fetch_err), Toast.LENGTH_SHORT).show()
                        }
                        spinner.visibility = View.GONE
                    },
                    {
                        error ->
                        Toast.makeText(context, "Fetching reader statistic error: ${error.message}", Toast.LENGTH_LONG).show()
                        spinner.visibility = View.GONE
                        Log.e("LoadReaderStat err", error.message)
                    })
        }
    }

    private fun updateReader(response: ReaderStatResponse) {
        name.text = response.name
        description.text = response.description
        isEntrance.text = if (response.isEntrance) context!!.getString(R.string.yes) else context!!.getString(R.string.no)
    }
}
