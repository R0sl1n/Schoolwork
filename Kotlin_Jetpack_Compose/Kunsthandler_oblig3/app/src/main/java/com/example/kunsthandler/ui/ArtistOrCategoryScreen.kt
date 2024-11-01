package com.example.kunsthandler.ui

import android.annotation.SuppressLint
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.navigation.NavController
import androidx.navigation.NavHostController
import androidx.navigation.compose.rememberNavController
import com.example.kunsthandler.R
import com.example.kunsthandler.datasource.ArtForSale
import com.example.kunsthandler.datasource.ChoiceListing
import com.example.kunsthandler.ui.theme.KunstHandelTheme
import androidx.lifecycle.viewmodel.compose.viewModel


@Composable
fun ArtistOrCategoryScreen(
    list: List<ChoiceListing>,
    viewModel: KunstViewModel,
    navController: NavController
){
    Column(modifier = Modifier.padding(16.dp),
        horizontalAlignment = Alignment.CenterHorizontally) {
        LazyColumn {
            items(list) { listItem ->
                ChoiceListing(
                    listItem = listItem,
                    onClick = { viewModel.updatePreviousScreen(listItem.name)
                        viewModel.updateAppBarText()
                        navController.navigate("listPhotos") },
                )

            }
        }
    }
    


}

@SuppressLint("ResourceType")
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun ChoiceListing(
    listItem: ChoiceListing,
    onClick: () -> Unit,
){
    Card(
        modifier = Modifier
            .padding(vertical = 8.dp)
            .fillMaxWidth(),
        onClick = onClick,
        elevation = CardDefaults.cardElevation(defaultElevation = 4.dp),
        shape = RoundedCornerShape(0)
    ) {
        Row {
            Image(
                painterResource(listItem.photo),
                contentDescription ="photo",
                modifier = Modifier.size(90.dp).weight(1f))
            Column(modifier = Modifier.weight(1f)) {
                Text(
                    text = stringResource(listItem.name),
                    style = MaterialTheme.typography.bodyMedium)
                Text(
                    text = stringResource(R.string.choice_listing_num_photos,listItem.numOfPhotos),
                    style = MaterialTheme.typography.bodySmall)
                Text(
                    text = stringResource(R.string.choice_listing_most_expensive,listItem.mostExpensivePhoto),
                    style = MaterialTheme.typography.bodySmall)
                Text(
                    text = stringResource(R.string.choice_listing_most_popular,stringResource(listItem.mostPopular)),
                    style = MaterialTheme.typography.bodySmall)

            }


        }




}}

@Preview(showBackground = true)
@Composable
fun ArtistOrCategoryScreenPreview(
    navController: NavHostController = rememberNavController(),
    viewModel: KunstViewModel = viewModel()
){
    KunstHandelTheme {
        ArtistOrCategoryScreen(
            list = ArtForSale.buildListing("artist"), viewModel, navController)
            

    }
}

