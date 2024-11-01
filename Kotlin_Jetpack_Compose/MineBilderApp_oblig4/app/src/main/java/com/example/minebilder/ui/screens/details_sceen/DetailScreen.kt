package com.example.minebilder.ui.screens.details_sceen

import android.content.res.Configuration
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.aspectRatio
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.ui.Modifier
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import coil.compose.rememberAsyncImagePainter
import com.example.minebilder.R
import com.example.minebilder.model.DownloadedPhoto
import com.example.minebilder.ui.MineBilderViewModel
import com.example.minebilder.ui.theme.MineBilderTheme


@Composable
fun DetailScreen(
    photoId: Int,
    viewModel: MineBilderViewModel,
) {
    val photoList = viewModel.photoUiState.collectAsState().value.downloadedPhotoList
    val photo = photoList.find { it.id == photoId }
    val albumTitle = remember { mutableStateOf("Loading...") }

    LaunchedEffect(photoId) {
        photo?.let {
            viewModel.fetchAlbumTitle(it.albumId)
            viewModel.albumTitle.collect { title ->
                albumTitle.value = title
            }
        }
    }
        DetailContent(photo, albumTitle.value)
}


@Composable
fun DetailContent(
    photo: DownloadedPhoto?,
    albumTitle: String
) {


    Column(modifier = Modifier
        .padding(16.dp)
        .verticalScroll(rememberScrollState())) {
        if (photo != null) {
            Card(
                modifier = Modifier.fillMaxWidth(),
                colors = CardDefaults.cardColors(MaterialTheme.colorScheme.primary)
            ) {
                Image(
                    painter = rememberAsyncImagePainter(photo.url, placeholder = painterResource(R.drawable.ic_launcher_foreground)),
                    contentDescription = "Photo",
                    modifier = Modifier
                        .fillMaxWidth()
                        .aspectRatio(1f),
                    contentScale = ContentScale.Crop
                )
            }
            Spacer(modifier = Modifier.height(16.dp))
            Card(
                modifier = Modifier.fillMaxWidth(),
                colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.primary)
            ) {
                Column(modifier = Modifier.padding(8.dp)) {
                    Text(stringResource(R.string.Id)+": ${photo.id}",
                        style = MaterialTheme.typography.bodyLarge,
                        color = MaterialTheme.colorScheme.onPrimary)
                    Text(stringResource(R.string.title)+": ${photo.title}",
                        style = MaterialTheme.typography.bodyLarge,
                        color = MaterialTheme.colorScheme.onPrimary,
                        maxLines = 1,
                        overflow = TextOverflow.Ellipsis)
                    Text(stringResource(R.string.Album_Id)+": ${photo.albumId}",
                        style = MaterialTheme.typography.bodyLarge,
                        color = MaterialTheme.colorScheme.onPrimary)
                    Text(stringResource(R.string.album_title)+": $albumTitle",
                        style = MaterialTheme.typography.bodyLarge,
                        color = MaterialTheme.colorScheme.onPrimary,
                        maxLines = 1,
                        overflow = TextOverflow.Ellipsis)
                }
            }
        } else {
            Text(stringResource(R.string.photo_not_found),
                style = MaterialTheme.typography.displayLarge,
                color = MaterialTheme.colorScheme.onSurface)
        }
    }
}


@Preview(showBackground = true, showSystemUi = true)
@Composable
fun DetailContentLightmodePreview(){
    val mockedDownloadPhoto =  DownloadedPhoto(0, 0,"Fake title","","")

    MineBilderTheme {
        Surface(modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background) {
        DetailContent(photo = mockedDownloadPhoto , albumTitle = "Fake title")
    }}

}

@Preview(showBackground = true, showSystemUi = true, uiMode = Configuration.UI_MODE_NIGHT_YES)
@Composable
fun DetailContentDarkModePreview(){
    val mockedDownloadPhoto =  DownloadedPhoto(0, 0,"Fake title","","")

    MineBilderTheme {
        Surface(modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background) {
            DetailContent(photo = mockedDownloadPhoto , albumTitle = "Fake title")
        }

    }

}