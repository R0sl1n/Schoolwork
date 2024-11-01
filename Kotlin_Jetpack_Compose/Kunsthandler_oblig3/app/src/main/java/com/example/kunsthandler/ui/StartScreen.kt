package com.example.kunsthandler.ui

import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.annotation.StringRes
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.defaultMinSize
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.example.kunsthandler.R
import com.example.kunsthandler.model.ShoppingCartItem
import androidx.navigation.NavHostController
import androidx.navigation.compose.rememberNavController
import com.example.kunsthandler.datasource.ArtForSale
import com.example.kunsthandler.ui.theme.KunstHandelTheme


@Composable
fun StartScreen(
    onArtistButtonClick: () -> Unit,
    onCategoryButtonClick: () -> Unit,
    onPaymentButtonClick: () -> Unit,
    viewModel: KunstViewModel,
    cart: List<ShoppingCartItem>,
    totalPrice: Double,
){

    Column(modifier = Modifier
        .padding(16.dp)
        .fillMaxSize()
        .verticalScroll(rememberScrollState()),
        horizontalAlignment = Alignment.CenterHorizontally) {
        Text(
            text = stringResource(R.string.start_screen_choose_picture_text),
            style = MaterialTheme.typography.displayLarge)
        Row(modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.SpaceEvenly) {
            StartScreenButton(
                buttonLabel = R.string.btn_artist,
                onClick = onArtistButtonClick,
                modifier = Modifier
                    .padding(horizontal = 10.dp, vertical = 20.dp)
                    .height(50.dp)
                    .width(150.dp))

            StartScreenButton(
                buttonLabel = R.string.btn_category,
                onClick = onCategoryButtonClick,
                modifier = Modifier
                    .padding(horizontal = 10.dp, vertical = 20.dp)
                    .height(50.dp)
                    .width(150.dp))
        }

        Column(modifier = Modifier.fillMaxWidth(),

            horizontalAlignment = Alignment.CenterHorizontally) {
            Text(stringResource(R.string.my_shopping_cart),
                style = MaterialTheme.typography.labelLarge)
            Text(stringResource(R.string.number_of_pictures, cart.size),
                style = MaterialTheme.typography.labelLarge)
            Text(stringResource(R.string.total_price, totalPrice),
                style = MaterialTheme.typography.labelLarge)
        }
        if(cart.isNotEmpty()){
            Column {
                cart.forEach { item ->
                    ShoppingListing(cartItem = item, viewModel = viewModel)

                }
            }

        }
        else{
            Spacer(modifier = Modifier.height(20.dp))
            Text(
                text = stringResource(R.string.empty_cart),
                style = MaterialTheme.typography.labelLarge)
        }
        Spacer(modifier = Modifier.weight(1f))
        StartScreenButton(
            buttonLabel = R.string.btn_go_to_payment,
            onClick = onPaymentButtonClick,
            modifier = Modifier
                .fillMaxWidth()
                .padding(horizontal = 10.dp, vertical = 10.dp)
        ,enabled = cart.isNotEmpty())
    }

}

@Composable
fun StartScreenButton(
    @StringRes buttonLabel: Int,
    onClick: () -> Unit,
    modifier: Modifier = Modifier,
    enabled: Boolean = true
){
    Button(
        onClick = onClick,
        modifier = modifier,
        colors = ButtonDefaults.buttonColors(MaterialTheme.colorScheme.primary),
        shape = RoundedCornerShape(0),
        contentPadding = PaddingValues(0.dp),
        enabled = enabled)
    {
        Text(
            text = stringResource(buttonLabel),
            style = MaterialTheme.typography.labelMedium)

    }
}

@Composable
fun ShoppingListing(
    cartItem: ShoppingCartItem,
    viewModel: KunstViewModel
){

    Card(
        modifier = Modifier
            .padding(vertical = 8.dp)
            .fillMaxWidth(),
        elevation = CardDefaults.cardElevation(defaultElevation = 4.dp),
        shape = RoundedCornerShape(0)
    ) {
        Row(horizontalArrangement = Arrangement.Absolute.Right) {
            Image(
                painterResource(cartItem.photo.imageRes),
                contentDescription ="photo",
                modifier = Modifier.size(90.dp),
                contentScale = ContentScale.FillBounds)
            Column {
            Text(
                text = stringResource(R.string.cart_item_name,stringResource(cartItem.photo.title)),
                style = MaterialTheme.typography.bodySmall)
            Text(
                text = stringResource(
                    R.string.cart_item_frame_text,
                    stringResource(cartItem.frameType.first,
                        cartItem.frameType.second),
                    stringResource(cartItem.frameWidth.first,cartItem.frameWidth.second),
                    stringResource(cartItem.frameSize.first,cartItem.frameSize.second)),
                style = MaterialTheme.typography.bodySmall)
            Text(
                text = stringResource(R.string.cart_item_total_price,cartItem.totalPrice),
                style = MaterialTheme.typography.bodySmall)
            }
            Spacer(modifier = Modifier.weight(1f))

            StartScreenButton(
                buttonLabel = R.string.btn_delete,
                modifier = Modifier
                    .defaultMinSize(minWidth = 1.dp, minHeight = 1.dp)
                    .height(35.dp)
                    .width(100.dp),
                onClick = { viewModel.removeFromShoppingCart(cartItem.id,cartItem.totalPrice) })




        }
    }
}



@Preview(showBackground = true)
@Composable
fun StartScreenPreview(
    viewModel: KunstViewModel = viewModel(),
    navController: NavHostController = rememberNavController()
){

    val list = listOf(
        ShoppingCartItem(
            id = ArtForSale.listOfArt[2].id,
            photo = ArtForSale.listOfArt[31],
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
    val totalPrice = list.map{it.totalPrice}.sumOf { it }


    KunstHandelTheme {
        StartScreen(
            viewModel = viewModel,
            onPaymentButtonClick = {},
            onCategoryButtonClick = {},
            onArtistButtonClick = {},
            cart = list,
            totalPrice = totalPrice,
            )

    }
}