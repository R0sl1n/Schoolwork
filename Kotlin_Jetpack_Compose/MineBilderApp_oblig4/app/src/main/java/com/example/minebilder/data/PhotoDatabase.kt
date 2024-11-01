package com.example.minebilder.data

import android.content.Context
import androidx.room.Database
import androidx.room.Room
import androidx.room.RoomDatabase
import com.example.minebilder.model.DownloadedPhoto
import com.example.minebilder.model.SavedPhoto

@Database(entities = [DownloadedPhoto::class ,SavedPhoto::class], version = 1)
abstract class PhotoDatabase: RoomDatabase() {

    abstract fun photoDao(): PhotoDao

    companion object {
        @Volatile
        private var Instance: PhotoDatabase? = null

        fun getDatabase(context: Context): PhotoDatabase {
            return Instance ?: synchronized(this) {
                Room.databaseBuilder(context, PhotoDatabase::class.java, "photo_database")
                    .fallbackToDestructiveMigration()
                    .build()
                    .also { Instance = it }
            }
        }
    }




}