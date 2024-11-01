package com.example.minebilder.worker

import android.annotation.SuppressLint
import android.content.Context
import android.util.Log
import androidx.work.CoroutineWorker
import androidx.work.WorkerParameters
import com.example.minebilder.MineBilderApplication
import com.example.minebilder.network.MineBilderApiService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import retrofit2.HttpException
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.io.IOException

class DownloadWorker(val context: Context, parameters: WorkerParameters): CoroutineWorker(context, parameters) {
    @SuppressLint("RestrictedApi")
    override suspend fun doWork(): Result {

        val app = context as MineBilderApplication
        val repo = app.container.mineBilderRepository
        val retrofit = Retrofit.Builder()
            .baseUrl(MineBilderApiService.BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
            .create(MineBilderApiService::class.java)

        return withContext(Dispatchers.IO) {
            return@withContext try {
                try {
                    val resp = retrofit.getPhotos()
                    if(resp.isSuccessful){
                        val photos = resp.body()
                        if(!photos.isNullOrEmpty()){
                            photos.forEach{ photo -> repo.insertToDownloaded(photo)}
                            Result.Success()
                        } else Result.failure()

                    } else Result.failure()

                } catch (e: IOException){
                    Log.d("WorkerError","IOException: ${e.message}")
                    Result.failure()
                }
                catch (e: HttpException){
                    Log.d("WorkerError","HttpException: ${e.message}")

                    Result.failure()
                }
        }catch (e: Exception){
                Log.d("WorkerError","Exception: ${e.message}")
            Result.failure()
        }}

    }
}