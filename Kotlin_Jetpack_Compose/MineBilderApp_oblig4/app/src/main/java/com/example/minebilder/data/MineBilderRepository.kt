package com.example.minebilder.data

import android.content.Context
import androidx.lifecycle.LiveData
import androidx.work.OneTimeWorkRequestBuilder
import androidx.work.WorkInfo
import androidx.work.WorkManager
import androidx.work.WorkRequest
import com.example.minebilder.model.DownloadedPhoto
import com.example.minebilder.model.SavedPhoto
import com.example.minebilder.network.MineBilderApiService
import com.example.minebilder.worker.DownloadWorker
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.flow

interface MineBilderRepository {

    fun getAlbumTitle(albumId: Int): Flow<String>
    fun getAllDownloadedPhotosStream(): Flow<List<DownloadedPhoto>>

    fun getAllSavedPhotosStream(): Flow<List<SavedPhoto>>

    suspend fun isEmpty(): Boolean

    suspend fun insertToDownloaded(downloadedPhoto: DownloadedPhoto)

    suspend fun insertToSaved(savedPhoto: SavedPhoto)

    suspend fun deleteSavedPhoto(savedPhoto: SavedPhoto)

    suspend fun downloadPhotosFromSource(): LiveData<WorkInfo>


}

class MineBilderRepositoryImpl(
    private val photoDao: PhotoDao,
    private val apiService: MineBilderApiService,
    context: Context): MineBilderRepository {

    private val workManager = WorkManager.getInstance(context)


    override fun getAlbumTitle(albumId: Int): Flow<String> = flow {
        try {
            val response = apiService.getAlbumById(albumId)
            if (response.isSuccessful && response.body() != null) {
                emit(response.body()!!.title)
            } else {
                emit("Album title not found")
            }
        } catch (e: Exception) {
            emit("Error fetching album title")
        }
    }

    override suspend fun isEmpty(): Boolean = photoDao.isEmpty()
    override fun getAllDownloadedPhotosStream(): Flow<List<DownloadedPhoto>> = photoDao.getAllDownloaded()

    override fun getAllSavedPhotosStream(): Flow<List<SavedPhoto>> = photoDao.getAllSaved()

    override suspend fun insertToDownloaded(downloadedPhoto: DownloadedPhoto) = photoDao.insertToDownloaded(downloadedPhoto)

    override suspend fun insertToSaved(savedPhoto: SavedPhoto) = photoDao.insertToSaved(savedPhoto)

    override suspend fun deleteSavedPhoto(savedPhoto: SavedPhoto) = photoDao.delete(savedPhoto)

    //Starts the WorkManagers and returns a LiveData object used by viewmodel to keep track current status of DownloadWorker.
    override suspend fun downloadPhotosFromSource(): LiveData<WorkInfo> {
        val workerRequest: WorkRequest = OneTimeWorkRequestBuilder<DownloadWorker>().build()
        workManager.enqueue(workerRequest)
        return workManager.getWorkInfoByIdLiveData(workerRequest.id)

    }
    }