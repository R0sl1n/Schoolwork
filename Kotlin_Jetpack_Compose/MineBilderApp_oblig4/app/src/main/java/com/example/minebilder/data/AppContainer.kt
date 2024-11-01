package com.example.minebilder.data

import android.content.Context
//import androidx.room.Room
import com.example.minebilder.network.MineBilderApiService
//import com.example.minebilder.model.PhotoDatabase
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

interface AppContainer {
    val mineBilderRepository: MineBilderRepository
}

class AppContainerImpl(private val context: Context): AppContainer {

    private val retrofit: Retrofit = Retrofit.Builder()
        .baseUrl(MineBilderApiService.BASE_URL)
        .addConverterFactory(GsonConverterFactory.create())
        .build()

    private val apiService: MineBilderApiService = retrofit.create(MineBilderApiService::class.java)

    private val photoDao = PhotoDatabase.getDatabase(context).photoDao()

    override val mineBilderRepository: MineBilderRepository by lazy {
        MineBilderRepositoryImpl(photoDao, apiService, context)
    }
}
