package com.example.kunsthandler.ui

import androidx.lifecycle.ViewModel
import com.example.kunsthandler.R
import com.example.kunsthandler.model.KunstUiState
import com.example.kunsthandler.model.Photo
import com.example.kunsthandler.model.ShoppingCartItem
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import java.math.BigDecimal
import java.math.RoundingMode

private val priceFrameType = mapOf(
    R.string.frame_material_plastic to 78.0,
    R.string.frame_material_wood to 120.0,
    R.string.frame_material_metal to 170.0)
private val priceFrameWidth = mapOf(
    R.string.selected_photo_width_10 to 50.0,
    R.string.selected_photo_width_15 to 70.0,
    R.string.selected_photo_width_20 to 90.0)
private val priceSize = mapOf(
    R.string.small to 70.0,
    R.string.medium to 120.0,
    R.string.large to 170.0)

class KunstViewModel: ViewModel() {
    private val _uiState = MutableStateFlow(KunstUiState())
    val uiState: StateFlow<KunstUiState> = _uiState.asStateFlow()

    // updates the previous screen stack used for appbar text.
    fun updatePreviousScreen(nextScreen: Int){
        val previousScreen = uiState.value.previousScreen
        val newPreviousScreen = mutableListOf<Int>()
        newPreviousScreen.addAll(previousScreen)
        newPreviousScreen.add(nextScreen)

        _uiState.update { currentState ->
            currentState.copy(previousScreen = newPreviousScreen)
        }

    }
    // Updates the appbar Text by looking at previous screen stack
    fun updateAppBarText(){
        val previousScreen = uiState.value.previousScreen
        val currentScreen = previousScreen.last()

        _uiState.update { currentState ->
            currentState.copy(currentScreen = currentScreen)

    }}

    // Removes previous screen from stack when user clicks backbutton or arrow.
    fun updateAppBarTextBacktrack(){
        val previousScreen = uiState.value.previousScreen
        val newPreviousScreen = mutableListOf<Int>()
        newPreviousScreen.addAll(previousScreen)
        newPreviousScreen.removeLast()
        _uiState.update { currentState ->
            currentState.copy(previousScreen = newPreviousScreen)
        }
        updateAppBarText()

    }


    fun updateShoppingCart(
        id: Int,
        photo: Photo,
        frameType: Int,
        frameWidth: Int,
        frameSize: Int){

        val previousShoppingCart = _uiState.value.shoppingCartItems
        val previousTotalPrice = _uiState.value.totalPrice
        val typePrice = priceFrameType[frameType]!!
        val sizePrice = priceSize[frameSize]!!
        val widthPrice = priceFrameWidth[frameWidth]!!
        val totalPrice = formatPrice(photo.price + typePrice + sizePrice + widthPrice)
        val newList = mutableListOf<ShoppingCartItem>()
        newList.addAll(previousShoppingCart)
        newList.add(
            ShoppingCartItem(
            id = id,
            photo = photo,
            frameSize = Pair(frameSize,sizePrice),
            frameWidth = Pair(frameWidth,widthPrice),
            frameType = Pair(frameType,typePrice),
            totalPrice = totalPrice
            )
        )
        val newTotalPrice = formatPrice(previousTotalPrice + totalPrice)
        _uiState.update {currentState ->
            currentState.copy(shoppingCartItems = newList, totalPrice = newTotalPrice)
          }


    }


    fun resetOrder(){
        _uiState.value = KunstUiState()
    }

    fun afterAddingToCart(){
        val newPreviousScreen = mutableListOf(R.string.app_name)
        _uiState.update {currentState ->
            currentState.copy(previousScreen = newPreviousScreen)
        }
    }

    fun removeFromShoppingCart(id: Int, price: Double){
        val previousShoppingCart = _uiState.value.shoppingCartItems
        val previousTotalPrice = _uiState.value.totalPrice
        val newList = previousShoppingCart.filter { it.id != id }
        val newTotalPrice = formatPrice(previousTotalPrice - price)
        _uiState.update {currentState ->
            currentState.copy(shoppingCartItems = newList, totalPrice = newTotalPrice)
        }

    }
    //Used for formatting price to two decimal places
    fun formatPrice(price: Double): Double{
         return BigDecimal(price).setScale(2, RoundingMode.HALF_EVEN).toDouble()
    }

}