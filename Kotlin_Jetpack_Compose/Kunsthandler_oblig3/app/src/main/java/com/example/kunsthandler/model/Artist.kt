package com.example.kunsthandler.model

import androidx.annotation.DrawableRes
import androidx.annotation.StringRes

data class Artist(
    val id: Int,
    @StringRes val name: Int,
    var numberOfPhotos: Int = 0,
    @StringRes val mostPopularPhoto: Int,
    @DrawableRes val photo: Int
)
