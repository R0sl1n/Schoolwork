package com.example.kunsthandler.ui

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.Button
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavHostController
import androidx.navigation.compose.rememberNavController
import com.example.kunsthandler.R
import com.example.kunsthandler.datasource.ArtForSale
import com.example.kunsthandler.model.ShoppingCartItem
import com.example.kunsthandler.ui.theme.KunstHandelTheme

@Composable
fun PaymentScreen(
    shoppingCart: List<ShoppingCartItem>,
    totalPrice: Double,
    onClick: () -> Unit,
){
    var showDialog by rememberSaveable{ mutableIntStateOf(0) }

    Column(modifier = Modifier
        .fillMaxSize()
        .verticalScroll(rememberScrollState())) {
        Column {
            shoppingCart.forEach { item ->
                PaymentListing(cartItem = item)
            }
            Text(modifier = Modifier.padding(start = 8.dp),
                text = stringResource(R.string.payment_screen_price_after_tax,totalPrice),
                style = MaterialTheme.typography.bodyMedium)

        }


    Card(modifier = Modifier
        .padding(vertical = 8.dp, horizontal = 8.dp)
        .fillMaxWidth(),
        elevation = CardDefaults.cardElevation(defaultElevation = 4.dp),
        shape = RoundedCornerShape(0)) {
        Column(modifier = Modifier.fillMaxWidth(),
            verticalArrangement = Arrangement.Center,
            horizontalAlignment = Alignment.CenterHorizontally) {
            Text(stringResource(R.string.payment_screen_payment_choice))
            Text(stringResource(R.string.payment_screen_card_nr, "5425233430109903"))
            Text(stringResource(R.string.payment_screen_card_expiry_date, "04/2026"))
            Text(stringResource(R.string.payment_screen_card_3_numbers, "123"))

            Button(
                onClick = { showDialog = 1}) {
                Text(stringResource(R.string.payment_screen_pay))
                
            }

        }
    }


    }
    if(showDialog != 0){
        ShowAlertDialog( onClick = onClick)
    }
}

@Composable
fun PaymentListing(
    cartItem: ShoppingCartItem
){
    Card(
        modifier = Modifier
            .padding(vertical = 8.dp, horizontal = 8.dp)
            .fillMaxWidth(),
        elevation = CardDefaults.cardElevation(defaultElevation = 4.dp),
        shape = RoundedCornerShape(0)
    ) {
        Column {
            Text(
                text = stringResource(R.string.cart_item_name,stringResource(cartItem.photo.title)),
                style = MaterialTheme.typography.bodySmall)
            Text(
                text = stringResource(R.string.cart_item_frame_text,stringResource(cartItem.frameType.first,cartItem.frameType.second),stringResource(cartItem.frameWidth.first,cartItem.frameWidth.second),stringResource(cartItem.frameSize.first,cartItem.frameSize.second)),
                style = MaterialTheme.typography.bodySmall)
            Text(
                text = stringResource(R.string.cart_item_total_price,cartItem.totalPrice),
                style = MaterialTheme.typography.bodySmall)
        }
}

}



@Composable
fun ShowAlertDialog(
    onClick: () -> Unit
){
    AlertDialog(
        onDismissRequest = onClick,
        confirmButton = {
            Button(onClick = onClick) {
                Text(text = stringResource(R.string.payment_screen_ok))
            }
        },
        title = { Text(text = stringResource(R.string.payment_screen_payment_completed)) },
        text = {
            Text(
                text = stringResource(R.string.payment_screen_payment_completed_details)
            )
        })
}

@Preview(showBackground = true)
@Composable
fun PaymentScreenPreview(
    viewModel: KunstViewModel = viewModel(),
    navController: NavHostController = rememberNavController()
){

    val list = listOf(
        ShoppingCartItem(
            id = ArtForSale.listOfArt[1].id,
            photo = ArtForSale.listOfArt[1],
            frameType = Pair(R.string.frame_material_plastic,78.0),
            frameSize = Pair(R.string.small,70.0),
            frameWidth = Pair(R.string.selected_photo_width_20,90.0),
            totalPrice = 132.54),
        ShoppingCartItem(
            id = ArtForSale.listOfArt[0].id,
            photo = ArtForSale.listOfArt[0],
            frameType = Pair(R.string.frame_material_wood,120.0),
            frameSize = Pair(R.string.medium,120.0),
            frameWidth = Pair(R.string.selected_photo_width_15,70.0),
            totalPrice = 432.54)
    )


    KunstHandelTheme {
        PaymentScreen(shoppingCart = list, onClick = {}, totalPrice = 172.54)

    }
}