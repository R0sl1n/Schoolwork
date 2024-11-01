package com.example.kunsthandler.ui

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.selection.selectable
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.Button
import androidx.compose.material3.Card
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.RadioButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableDoubleStateOf
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavController
import androidx.navigation.NavHostController
import androidx.navigation.compose.rememberNavController
import com.example.kunsthandler.R
import com.example.kunsthandler.datasource.ArtForSale
import com.example.kunsthandler.model.Photo
import com.example.kunsthandler.ui.theme.KunstHandelTheme

@Composable
fun SelectedPhotoScreen(
    photo: Photo,
    viewModel: KunstViewModel,
    navController: NavController
){
    var selectedSize by rememberSaveable { mutableIntStateOf(0) }
    var selectedType by rememberSaveable{ mutableIntStateOf(0) }
    var selectedWidth by rememberSaveable { mutableIntStateOf(0) }
    var totalPrice by rememberSaveable { mutableDoubleStateOf(photo.price) }
    val frameType = listOf(
        Pair(R.string.frame_material_wood,120),
        Pair(R.string.frame_material_metal,170),
        Pair(R.string.frame_material_plastic,78))
    val frameSize = listOf(
        Pair(R.string.small,70),
        Pair(R.string.medium,120),
        Pair(R.string.large,170))
    val frameThickness = listOf(
        Pair(R.string.selected_photo_width_10,50),
        Pair(R.string.selected_photo_width_15,70),
        Pair(R.string.selected_photo_width_20,90))




    Column(modifier = Modifier
        .fillMaxSize()
        .verticalScroll(rememberScrollState())) {
        Column(horizontalAlignment = Alignment.CenterHorizontally,
            modifier = Modifier
                .fillMaxWidth()) {
            Card(
                modifier = Modifier
                    .background(MaterialTheme.colorScheme.primaryContainer)
                    .padding(8.dp)
            ) {
                Image(
                    modifier = Modifier.size(200.dp),
                    painter = painterResource(photo.imageRes),
                    contentDescription = stringResource(photo.title)
                )
            }
        }
        Column(modifier = Modifier
            .fillMaxWidth()
            .padding(5.dp)){
            Card {

            Column(modifier = Modifier.fillMaxWidth(),
                horizontalAlignment = Alignment.CenterHorizontally) {


                val artistName = photo.artist?.let { stringResource(it.name) }.toString()
                val price = photo.price
                Text(text = stringResource(R.string.selected_photo_details))
                Text(text = stringResource(R.string.selected_photo_artist,artistName))
                Text(text = stringResource(R.string.selected_photo_price,price))
                Text(text = stringResource(R.string.selected_photo_total_price,viewModel.formatPrice(totalPrice)))
                Text(text = stringResource(R.string.selected_photo_frame_selection))
            }
            Row(modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.Center,
                verticalAlignment = Alignment.CenterVertically) {

            Column {
                Text(stringResource(R.string.selected_photo_frame_material),
                    style = MaterialTheme.typography.labelLarge)
            frameType.forEach { item ->
                RadioButtonListing(
                    selected = selectedType == item.first,
                    onClick = {
                        selectedType = item.first
                        totalPrice += item.second
                    },
                    text = stringResource(item.first,item.second),
                    modifier = Modifier.selectable(
                        selected = selectedType == item.first,
                        onClick = {
                            selectedType = item.first
                            totalPrice += item.second
                        }
                    ))

            }}
                Column {
                    Text(stringResource(R.string.chosen_frame_size),
                        style = MaterialTheme.typography.labelLarge)

            frameSize.forEach { item ->
                RadioButtonListing(
                    selected = selectedSize == item.first,
                    onClick = {
                        selectedSize = item.first
                        totalPrice += item.second
                    },
                    text =stringResource(item.first,item.second),
                    modifier = Modifier.selectable(
                        selected = selectedSize == item.first,
                        onClick = {
                            selectedSize = item.first
                            totalPrice += item.second
                        }
                    ))

            }}
            Column {
                Text(
                    stringResource(R.string.chosen_frame_width),
                    style = MaterialTheme.typography.labelLarge)

            frameThickness.forEach { item ->
                RadioButtonListing(
                    selected = selectedWidth == item.first,
                    onClick = {
                        selectedWidth = item.first
                        totalPrice += item.second
                    },
                    text = stringResource(item.first,item.second),
                    modifier = Modifier
                        .selectable(
                            selected = selectedWidth == item.first,
                            onClick = {
                                selectedWidth = item.first
                                totalPrice += item.second
                            }
                        )
                        .padding(end = 5.dp))
            }}

            }
                Row(modifier = Modifier.fillMaxWidth(),
                    horizontalArrangement = Arrangement.Center) {


                Button(
                    shape = RoundedCornerShape(0),
                    contentPadding = PaddingValues(8.dp),
                    onClick = { addPhotoToCart(photo,viewModel,selectedType,selectedSize,selectedWidth)
                        viewModel.afterAddingToCart()
                        viewModel.updateAppBarText()
                        navController.popBackStack("StartScreen", inclusive = false)},
                    enabled = (selectedSize != 0 && selectedWidth !=0 && selectedType !=0)) {
                    Text(text = stringResource(R.string.add_to_shopping_cart))

                }
                }

        }
        }
        Spacer(modifier = Modifier.weight(1f))
        Button(modifier = Modifier.fillMaxWidth(),
            shape = RoundedCornerShape(0),
            contentPadding = PaddingValues(0.dp),
            onClick = { navController.popBackStack("StartScreen", inclusive = false) }) {
            Text(text = stringResource(R.string.btn_home))
            
        }

        }
    }

@Composable
fun RadioButtonListing(
    selected: Boolean,
    onClick: () -> Unit,
    text: String,
    modifier: Modifier = Modifier)
  {

    Row(
        modifier = modifier,
        verticalAlignment = Alignment.CenterVertically
    ) {
        RadioButton(
            modifier = Modifier.size(35.dp),
            selected = selected,
            onClick = onClick
        )
        Text(
            text = text,
            style = MaterialTheme.typography.labelMedium)

    }

}

private fun addPhotoToCart(
    photo: Photo,
    viewModel: KunstViewModel,
    selectedType: Int,
    selectedSize: Int,
    selectedWidth: Int
){

    viewModel.updateShoppingCart(
            id = photo.id,
            photo = photo,
            frameType = selectedType,
            frameWidth = selectedWidth,
            frameSize = selectedSize)

}

@Preview(showBackground = true)
@Composable
fun SelectedPhotoScreenPreview(
    viewModel: KunstViewModel = viewModel(),
    navController: NavHostController = rememberNavController()
){
    KunstHandelTheme {
        SelectedPhotoScreen(
            photo = ArtForSale.listOfArt[0], viewModel = viewModel,navController = navController)

    }
}