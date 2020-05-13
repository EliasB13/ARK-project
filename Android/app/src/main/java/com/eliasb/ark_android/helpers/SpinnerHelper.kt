package com.eliasb.ark_android.helpers

import android.view.View
import android.widget.Button
import android.widget.ProgressBar

class SpinnerHelper {
    companion object {
        fun showSpinner(spinner: View, btn: Button) {
            spinner.visibility = View.VISIBLE
            btn.visibility = View.GONE
        }

        fun dismissSpinner(spinner: View, btn: Button) {
            spinner.visibility = View.GONE
            btn.visibility = View.VISIBLE
        }
    }
}