package com.eliasb.ark_android.ui.statistic

import android.content.Context
import android.net.Uri
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.LinearLayoutManager

import com.eliasb.ark_android.R
import com.eliasb.ark_android.adapters.ReadersAdapter
import kotlinx.android.synthetic.main.fragment_common_statistic.*

class CommonStatisticFragment : Fragment() {
    private lateinit var adapter: ReadersAdapter

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {

        return inflater.inflate(R.layout.fragment_common_statistic, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)

        adapter = ReadersAdapter(context!!)
        adapter.refreshReaders(empty_list, readers_list)

        readers_list.layoutManager = LinearLayoutManager(context)
        readers_list.adapter = adapter
    }
}
