package com.example.minebilder.data

import androidx.room.Dao
import androidx.room.Delete
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import com.example.minebilder.model.DownloadedPhoto
import com.example.minebilder.model.SavedPhoto
import kotlinx.coroutines.flow.Flow

@Dao
interface PhotoDao{

    @Query("SELECT * from downloadedphotos")
    fun getAllDownloaded(): Flow<List<DownloadedPhoto>>

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun insertToDownloaded(photo: DownloadedPhoto)

    @Query("SELECT * from savedphotos")
    fun getAllSaved(): Flow<List<SavedPhoto>>

    @Query("SELECT (SELECT COUNT(*) FROM downloadedphotos)== 0")
    suspend fun isEmpty(): Boolean

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun insertToSaved(savedPhoto: SavedPhoto)

    @Delete
    suspend fun delete(savedPhoto: SavedPhoto)
}
