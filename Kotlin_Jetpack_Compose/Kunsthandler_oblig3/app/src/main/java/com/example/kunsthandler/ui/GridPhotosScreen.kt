package com.example.kunsthandler.ui

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.grid.GridCells
import androidx.compose.foundation.lazy.grid.LazyVerticalGrid
import androidx.compose.foundation.lazy.grid.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.ExperimentalMaterial3Api
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
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavController
import androidx.navigation.NavHostController
import androidx.navigation.compose.rememberNavController
import com.example.kunsthandler.R
import com.example.kunsthandler.datasource.ArtForSale
import com.example.kunsthandler.model.Photo
import com.example.kunsthandler.ui.theme.KunstHandelTheme


@Composable
fun GridPhotosScreen(
    photoList: List<Photo>,
    viewModel: KunstViewModel,
    navController: NavController
){
    LazyVerticalGrid(modifier = Modifier.padding(8.dp),
        verticalArrangement = Arrangement.spacedBy(8.dp),
        horizontalArrangement = Arrangement.spacedBy(8.dp),
        columns = GridCells.Fixed(2) ){
        items(photoList){photo ->
            PhotoCard(
                photo = photo, onClick = {
                    viewModel.updatePreviousScreen(photo.title)
                    viewModel.updateAppBarText()
                    navController.navigate("selectedPhoto")})
        }
    }

}

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun PhotoCard(
    photo: Photo,
    onClick: () -> Unit,
) {


    Card(
        modifier = Modifier,
        onClick = onClick,
        elevation = CardDefaults.cardElevation(defaultElevation = 4.dp),
        shape = RoundedCornerShape(0)
    ) {

    Box(modifier = Modifier.fillMaxWidth(), contentAlignment = Alignment.TopCenter) {
        Image(
            modifier = Modifier.size(200.dp),
            painter = painterResource(photo.imageRes),
            contentDescription = stringResource(photo.title),
            contentScale = ContentScale.Crop
        )
        Text(
            modifier = Modifier.background(MaterialTheme.colorScheme.onPrimary),
            text = stringResource(photo.title),
            style = MaterialTheme.typography.labelLarge
        )
    }
}


}

@Preview(showBackground = true)
@Composable
fun GridPhotosPreview(
    navController: NavHostController = rememberNavController(),
    viewModel: KunstViewModel = viewModel()
){
    KunstHandelTheme {
        GridPhotosScreen(
            photoList = ArtForSale.getPhotos(R.string.category_nature), viewModel = viewModel,navController = navController)


    }
}

