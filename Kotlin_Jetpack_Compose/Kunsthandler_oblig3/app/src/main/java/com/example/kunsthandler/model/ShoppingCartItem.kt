package com.example.kunsthandler.model

data class ShoppingCartItem(
    val id: Int,
    val photo: Photo,
    var frameType: Pair<Int,Double>,
    val frameWidth: Pair<Int,Double>,
    val frameSize: Pair<Int,Double>,
    val totalPrice: Double,
)
