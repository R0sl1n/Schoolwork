package com.example.kunsthandler.model


import com.example.kunsthandler.R
// Holds the state of the app
// previousScreen is a stack used to keep track of user choices for displaying the appbar text
data class KunstUiState(
    val previousScreen: MutableList<Int> = mutableListOf(R.string.app_name),
    val currentScreen: Int = R.string.app_name,
    val totalPrice: Double = 0.0,
    val shoppingCartItems: List<ShoppingCartItem> = listOf()
)