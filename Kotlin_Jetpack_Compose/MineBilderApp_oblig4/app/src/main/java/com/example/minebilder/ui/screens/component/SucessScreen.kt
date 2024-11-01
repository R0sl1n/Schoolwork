package com.example.minebilder.ui.screens.component

import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview
import androidx.navigation.compose.currentBackStackEntryAsState
import androidx.navigation.compose.rememberNavController
import com.example.minebilder.MineBilderAppBar
import com.example.minebilder.R
import com.example.minebilder.navigation.MineBilderNavHost
import com.example.minebilder.ui.MineBilderViewModel
import com.example.minebilder.ui.theme.MineBilderTheme

@Composable
fun SucessScreen(viewModel: MineBilderViewModel){
    val navController = rememberNavController()

    val backStackEntry by navController.currentBackStackEntryAsState()

    val currentScreen = when(backStackEntry?.destination?.route){
        "homescreen"-> R.string.app_name
        else -> R.string.valgt_bilde
    }


    val canNavigateBack = navController.previousBackStackEntry != null

    Scaffold(
        topBar = {
            MineBilderAppBar(
                currentScreen = currentScreen,
                canNavigateBack = canNavigateBack,
                onNavigateUpClicked = { navController.navigateUp()}
            )
        }
    ) {

        MineBilderNavHost(
            navController = navController,
            viewModel = viewModel,
            modifier = Modifier
                .fillMaxSize()
                .padding(it))
    }
}

@Preview(showBackground = true)
@Composable
fun TopBarPreview(){
    MineBilderTheme {
        MineBilderAppBar(R.string.app_name,true,{})

    }
}