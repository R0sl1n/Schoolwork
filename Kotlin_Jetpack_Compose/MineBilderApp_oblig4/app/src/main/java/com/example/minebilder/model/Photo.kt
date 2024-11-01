package com.example.minebilder.model

import androidx.room.Entity
import androidx.room.PrimaryKey
import kotlinx.serialization.Serializable

@Serializable
@Entity(tableName = "DownloadedPhotos")
data class DownloadedPhoto(
    val albumId: Int,
    @PrimaryKey(autoGenerate = false)
    val id: Int,
    val title: String,
    val url: String,
    val thumbnailUrl: String
)

@Entity(tableName = "SavedPhotos")
data class SavedPhoto(
    val albumId: Int,
    @PrimaryKey(autoGenerate = false)
    val id: Int,
    val title: String,
    val url: String,
    val thumbnailUrl: String
)

data class Album(
    val userId: Int,
    val id: Int,
    val title: String
)

fun DownloadedPhoto.toSavedPhoto(): SavedPhoto= SavedPhoto(
    albumId, id, title, url, thumbnailUrl
)