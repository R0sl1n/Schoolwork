package com.example.minebilder.navigation

import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.navigation.NavHostController
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import com.example.minebilder.R
import com.example.minebilder.ui.screens.home_screen.HomeScreen
import com.example.minebilder.ui.screens.details_sceen.DetailScreen
import com.example.minebilder.ui.MineBilderViewModel



@Composable
fun MineBilderNavHost(
    navController: NavHostController,
    viewModel: MineBilderViewModel,
    modifier: Modifier = Modifier
) {
    NavHost(
        navController = navController,
        startDestination = "homescreen",
        modifier = modifier
    ) {
        composable("homescreen") {
            HomeScreen(viewModel = viewModel, navController = navController)
        }
        composable("detail/{photoId}") { backStackEntry ->
            val photoId = backStackEntry.arguments?.getString("photoId")?.toIntOrNull()
            if (photoId != null) {
                DetailScreen(
                    photoId = photoId,
                    viewModel = viewModel
                )
            } else {
                Text(stringResource(R.string.error_missing_or_invalid_photo_id))
            }
        }
    }
}
