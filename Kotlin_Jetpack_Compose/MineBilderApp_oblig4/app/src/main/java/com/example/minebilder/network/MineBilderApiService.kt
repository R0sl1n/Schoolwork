package com.example.minebilder.network

import com.example.minebilder.model.Album
import com.example.minebilder.model.DownloadedPhoto
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Path

interface MineBilderApiService {


    @GET("photos")
    suspend fun getPhotos(): Response<List<DownloadedPhoto>>

    @GET("albums/{id}")
    suspend fun getAlbumById(@Path("id") albumId: Int): Response<Album>

    companion object {
        const val BASE_URL = "https://jsonplaceholder.typicode.com/"
    }


}