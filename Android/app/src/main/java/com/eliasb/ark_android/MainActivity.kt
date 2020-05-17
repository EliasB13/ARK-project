package com.eliasb.ark_android

import android.app.AlarmManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import android.view.Menu
import android.view.MenuItem
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.widget.Toolbar
import androidx.core.view.GravityCompat
import androidx.drawerlayout.widget.DrawerLayout
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.navigateUp
import androidx.navigation.ui.setupActionBarWithNavController
import androidx.navigation.ui.setupWithNavController
import com.eliasb.ark_android.services.PreferencesService
import com.eliasb.ark_android.services.RestrictedObservationsReceiver
import com.eliasb.ark_android.ui.LoginActivity
import com.google.android.material.navigation.NavigationView


class MainActivity : AppCompatActivity(), NavigationView.OnNavigationItemSelectedListener {

    private lateinit var appBarConfiguration: AppBarConfiguration

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        val toolbar: Toolbar = findViewById(R.id.toolbar)
        setSupportActionBar(toolbar)

        val drawerLayout: DrawerLayout = findViewById(R.id.drawer_layout)
        val navView: NavigationView = findViewById(R.id.nav_view)
        val navController = findNavController(R.id.nav_host_fragment)
        appBarConfiguration = AppBarConfiguration(
            setOf(
                R.id.nav_statistic,
                R.id.nav_profile,
                R.id.nav_restricted_obs
            ), drawerLayout
        )
        setupActionBarWithNavController(navController, appBarConfiguration)
        navView.setupWithNavController(navController)

        navView.setNavigationItemSelectedListener(this)

        checkAuthorization()
        scheduleAlarm()
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        menuInflater.inflate(R.menu.main, menu)
        return true
    }

    override fun onSupportNavigateUp(): Boolean {
        val navController = findNavController(R.id.nav_host_fragment)
        return navController.navigateUp(appBarConfiguration) || super.onSupportNavigateUp()
    }

    override fun onNavigationItemSelected(p0: MenuItem): Boolean {
        when (p0.itemId) {
            R.id.nav_statistic -> findNavController(R.id.nav_host_fragment).navigate(R.id.nav_statistic)
            R.id.nav_profile -> findNavController(R.id.nav_host_fragment).navigate(R.id.nav_profile)
            R.id.nav_restricted_obs -> findNavController(R.id.nav_host_fragment).navigate(R.id.nav_restricted_obs)
            R.id.nav_share -> {
                val share = Intent.createChooser(Intent().apply {
                    action = Intent.ACTION_SEND
                        putExtra(Intent.EXTRA_TEXT, "https://github.com/EliasB13/ARK-project")
                        type = "text/plain"
                    }, null)
                    startActivity(share)
                }
            R.id.nav_github -> {
                val intent = Intent(Intent.ACTION_VIEW)
                intent.data = Uri.parse("https://github.com/EliasB13/ARK-project")
                startActivity(intent)
            }
        }

        val drawerLayout: DrawerLayout = findViewById(R.id.drawer_layout)
        drawerLayout.closeDrawer(GravityCompat.START)
        return true
    }

    private fun checkAuthorization() {
        val prefService = PreferencesService
        prefService.create(this, getString(R.string.user_pref))
        val token = prefService.getPreference(getString(R.string.token))
        if (token == null) {
            val intent = Intent(this, LoginActivity::class.java)
            startActivity(intent)
            finish()
        }
    }

    private fun scheduleAlarm() {
        val intent = Intent(applicationContext, RestrictedObservationsReceiver::class.java)
        val pIntent = PendingIntent.getBroadcast(
            this, RestrictedObservationsReceiver.REQUEST_CODE,
            intent, PendingIntent.FLAG_UPDATE_CURRENT
        )
        val firstMillis =
            System.currentTimeMillis()
        val alarm = this.getSystemService(Context.ALARM_SERVICE) as AlarmManager
        alarm.setInexactRepeating(
            AlarmManager.RTC_WAKEUP, firstMillis,
            10000L, pIntent
        )
    }
}
