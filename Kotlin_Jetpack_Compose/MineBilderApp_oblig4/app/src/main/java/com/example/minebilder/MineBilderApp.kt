package com.example.minebilder

import androidx.annotation.StringRes
import androidx.compose.foundation.layout.Row
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBack
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.material3.TopAppBarDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.lifecycle.viewmodel.compose.viewModel
import com.example.minebilder.ui.MineBilderViewModel
import com.example.minebilder.ui.StatusUiState
import com.example.minebilder.ui.screens.component.ErrorScreen
import com.example.minebilder.ui.screens.component.LoadingScreen
import com.example.minebilder.ui.screens.component.SucessScreen

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun MineBilderAppBar(
    @StringRes currentScreen: Int,
    canNavigateBack: Boolean,
    onNavigateUpClicked: () -> Unit
){
    TopAppBar(
        title = { Text(stringResource(currentScreen),
            style = MaterialTheme.typography.titleLarge,
            color = MaterialTheme.colorScheme.onPrimaryContainer) },
        navigationIcon = {
            if(canNavigateBack){
            IconButton(onClick = onNavigateUpClicked) {
                Icon(
                    Icons.Filled.ArrowBack,
                    contentDescription = stringResource(R.string.go_back)
                )
            }
        }}, colors = TopAppBarDefaults.topAppBarColors(
            containerColor = MaterialTheme.colorScheme.primaryContainer
        ),
        actions = {Row {
            Icon(painter = painterResource(R.drawable.ic_launcher_foreground), contentDescription = "app icon")
        }}

    )

}

@Composable
fun MineBilderApp() {
    val viewModel: MineBilderViewModel = viewModel(factory = MineBilderViewModel.Factory)
    when(viewModel.uiStatusState.collectAsState().value){
        StatusUiState.Success -> SucessScreen(viewModel = viewModel)
        StatusUiState.Loading -> LoadingScreen(viewModel = viewModel)
        else -> ErrorScreen { viewModel.getPhotosFromWeb() }


    }

}


