package com.example.kunsthandler.model

import androidx.annotation.DrawableRes
import androidx.annotation.StringRes

data class Photo(
    val id: Int,
    @StringRes var title: Int,
    @DrawableRes val imageRes: Int,
    var artist: Artist?,
    var category: CategoryType,
    var price: Double = 0.0
)
