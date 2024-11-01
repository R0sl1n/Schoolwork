package com.example.minebilder.ui.screens.home_screen

import android.content.res.Configuration
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
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
fun CompactWindow(
    onDeleteClick: (SavedPhoto) -> Unit,
    onSaveClick: (DownloadedPhoto) -> Unit,
    savedPhotoList: List<SavedPhoto>,
    downloadedPhotoList: List<DownloadedPhoto>,
    navigate: (Int) -> Unit
){
    Column(horizontalAlignment = Alignment.CenterHorizontally){
        Column(horizontalAlignment = Alignment.CenterHorizontally,
            modifier = Modifier.weight(1f)) {
            Text(
                stringResource(R.string.saved_pictures),
                style = MaterialTheme.typography.labelLarge,
                color = MaterialTheme.colorScheme.onBackground
            )
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
        Column(horizontalAlignment = Alignment.CenterHorizontally,
            modifier = Modifier.weight(1f)) {
            Text(
                stringResource(R.string.dowloaded_pictures),
                style = MaterialTheme.typography.labelLarge,
                color = MaterialTheme.colorScheme.onBackground
            )
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

@Preview(showBackground = true, showSystemUi = true)
@Composable
fun CompactWindowLightModePreview(
    navController: NavHostController = rememberNavController()
){
    val mockedSavedData = List(10) { SavedPhoto(it, it,"Fake title","","") }
    val mockedDownloadedData = List(10) { DownloadedPhoto(it, it,"Fake title","","") }
    MineBilderTheme {
        Surface(modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background) {
        CompactWindow(
            onDeleteClick = {},
            onSaveClick = {},
            savedPhotoList = mockedSavedData ,
            downloadedPhotoList = mockedDownloadedData,
            navigate = {}
        )
    }}

}

@Preview(showBackground = true, showSystemUi = true,uiMode = Configuration.UI_MODE_NIGHT_YES)
@Composable
fun CompactWindowDarkModePreview(
    navController: NavHostController = rememberNavController()
){
    val mockedSavedData = List(10) { SavedPhoto(it, it,"Fake title","","") }
    val mockedDownloadedData = List(10) { DownloadedPhoto(it, it,"Fake title","","") }
    MineBilderTheme {
        Surface(modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background) {
        CompactWindow(
            onDeleteClick = {},
            onSaveClick = {},
            savedPhotoList = mockedSavedData ,
            downloadedPhotoList = mockedDownloadedData,
            navigate = {}
        )
    }}

}