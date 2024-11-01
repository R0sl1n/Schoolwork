package com.example.minebilder.ui.screens.home_screen

import android.content.res.Configuration
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.navigation.NavHostController
import androidx.navigation.compose.rememberNavController
import com.example.minebilder.R
import com.example.minebilder.model.DownloadedPhoto
import com.example.minebilder.model.SavedPhoto
import com.example.minebilder.ui.theme.MineBilderTheme

@Composable
fun LandscapeWindow(
    onDeleteClick: (SavedPhoto) -> Unit,
    onSaveClick: (DownloadedPhoto) -> Unit,
    savedPhotoList: List<SavedPhoto>,
    downloadedPhotoList: List<DownloadedPhoto>,
    navigate: (Int) -> Unit,
){
    Row {
        Column(modifier = Modifier.weight(1f), horizontalAlignment = Alignment.CenterHorizontally) {
            Text(
                stringResource(R.string.saved_pictures),
                style = MaterialTheme.typography.labelLarge,
                color = MaterialTheme.colorScheme.onSurface)
            PhotoList(
                photos = savedPhotoList,
                onPhotoClick = { photo ->
                    if (photo is SavedPhoto) {
                        navigate(photo.id)
                    }
                },
                onSaveClick = {},
                onDeleteClick = onDeleteClick
            )
        }
        Column(modifier = Modifier.weight(1f)) {
            Text(
                stringResource(R.string.dowloaded_pictures),
                style = MaterialTheme.typography.labelLarge,
                color = MaterialTheme.colorScheme.onSurface)
            PhotoList(
                photos = downloadedPhotoList,
                onPhotoClick = { photo ->
                    if (photo is DownloadedPhoto) {
                        navigate(photo.id)
                    }
                },
                onSaveClick = onSaveClick,
                onDeleteClick = {}
            )
        }
    }

}


@Preview(showBackground = true, showSystemUi = true,device = "spec:width=411dp,height=891dp,orientation=landscape")
@Composable
fun LandscapeWindowLightModePreview(
    navController: NavHostController = rememberNavController()
){
    val mockedSavedData = List(10) { SavedPhoto(it, it,"Fake title","","") }
    val mockedDownloadedData = List(10) { DownloadedPhoto(it, it,"Fake title","","") }
    MineBilderTheme {
        Surface(
            color = MaterialTheme.colorScheme.background) {
            LandscapeWindow(
                onDeleteClick = {},
                onSaveClick = {},
                savedPhotoList = mockedSavedData ,
                downloadedPhotoList = mockedDownloadedData,
                navigate = {}
            )
        }
    }

}

@Preview(showBackground = true,device = "spec:width=411dp,height=891dp,orientation=landscape", showSystemUi = true, uiMode = Configuration.UI_MODE_NIGHT_YES)
@Composable
fun LandscapeWindowDarkModePreview(
    navController: NavHostController = rememberNavController()
){
    val mockedSavedData = List(10) { SavedPhoto(it, it,"Fake title","","") }
    val mockedDownloadedData = List(10) { DownloadedPhoto(it, it,"Fake title","","") }
    MineBilderTheme {
        Surface(
            color = MaterialTheme.colorScheme.background) {
            LandscapeWindow(
                onDeleteClick = {},
                onSaveClick = {},
                savedPhotoList = mockedSavedData ,
                downloadedPhotoList = mockedDownloadedData,
                navigate = {}
            )
        }
    }

}