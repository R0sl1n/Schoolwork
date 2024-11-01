package com.example.minebilder.ui.screens.home_screen

import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.ui.platform.LocalConfiguration
import android.content.res.Configuration
import androidx.compose.runtime.getValue
import androidx.navigation.NavController
import com.example.minebilder.ui.MineBilderViewModel

@Composable
fun HomeScreen(
    viewModel: MineBilderViewModel,
    navController: NavController,
) {
    val photosUiState by viewModel.photoUiState.collectAsState()
    val configuration = LocalConfiguration.current
    when(configuration.orientation){
        Configuration.ORIENTATION_PORTRAIT -> CompactWindow(
            onSaveClick = {viewModel.savePhoto(it)},
            onDeleteClick = {viewModel.delete(it)},
            downloadedPhotoList = photosUiState.downloadedPhotoList,
            savedPhotoList = photosUiState.savedPhotoList,
            navigate = {navController.navigate("detail/${it}")}
        )
        else -> LandscapeWindow(
            onSaveClick = {viewModel.savePhoto(it)},
            onDeleteClick = {viewModel.delete(it)},
            downloadedPhotoList = photosUiState.downloadedPhotoList,
            savedPhotoList = photosUiState.savedPhotoList,
            navigate = {navController.navigate("detail/${it}")}
        )
    }
}