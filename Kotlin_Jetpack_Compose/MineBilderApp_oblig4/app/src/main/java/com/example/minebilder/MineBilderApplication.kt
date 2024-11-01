package com.example.minebilder

import android.app.Application
import com.example.minebilder.data.AppContainer
import com.example.minebilder.data.AppContainerImpl

class MineBilderApplication: Application() {

    lateinit var container: AppContainer
    override fun onCreate() {
        super.onCreate()
        container = AppContainerImpl(this)
    }
}
