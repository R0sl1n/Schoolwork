package com.example.kunsthandler

import android.annotation.SuppressLint
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.ArrowBack
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.material3.TopAppBarDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import com.example.kunsthandler.ui.KunstViewModel
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.compose.rememberNavController
import androidx.navigation.NavHostController
import androidx.navigation.compose.NavHost
import androidx.compose.runtime.collectAsState
import androidx.compose.ui.Alignment
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.navigation.compose.composable
import com.example.kunsthandler.datasource.ArtForSale
import com.example.kunsthandler.datasource.ChoiceListing
import com.example.kunsthandler.ui.SelectedPhotoScreen
import com.example.kunsthandler.ui.GridPhotosScreen
import com.example.kunsthandler.ui.StartScreen
import com.example.kunsthandler.ui.ArtistOrCategoryScreen
import com.example.kunsthandler.ui.PaymentScreen

@Composable
@OptIn(ExperimentalMaterial3Api::class)
fun KunstHandelAppBar(
    currentScreen: Int,
    canNavigateBack: Boolean,
    navigateUp: () -> Unit,
    modifier: Modifier = Modifier
) {
    TopAppBar(
        title = {
            Box(modifier = Modifier.fillMaxWidth()) {
                Text(
                    text = stringResource(currentScreen),
                    style = MaterialTheme.typography.displayMedium,
                    modifier = Modifier,
                )
            }
        },
        colors = TopAppBarDefaults.mediumTopAppBarColors(
            containerColor = MaterialTheme.colorScheme.primaryContainer
        ),
        modifier = modifier,
        navigationIcon = {
            if (canNavigateBack) {
                IconButton(onClick = navigateUp) {
                    Icon(
                        imageVector = Icons.AutoMirrored.Filled.ArrowBack,
                        contentDescription = stringResource(R.string.back_button_text)
                    )
                }
            }
        },
        actions = {
            Box(
                modifier = Modifier
                    .fillMaxHeight() //
                    .padding(horizontal = 12.dp),
                contentAlignment = Alignment.Center
            ) {
                Icon(
                    painter = painterResource(id = R.drawable.yell),
                    contentDescription = null,
                    modifier = Modifier.size(150.dp)
                )
            }

        }
    )

}

@SuppressLint("MutableCollectionMutableState", "RestrictedApi", "StateFlowValueCalledInComposition")
@Composable
fun KunstHandelApp(
    viewModel: KunstViewModel = viewModel(),
    navController: NavHostController = rememberNavController()
){
    val uiState by viewModel.uiState.collectAsState()


    Scaffold(
        topBar = {
            KunstHandelAppBar(
                currentScreen = uiState.currentScreen,
                canNavigateBack = navController.previousBackStackEntry != null,
                navigateUp = {
                    navController.navigateUp()
                    }
            )
        }
    ) { innerPadding ->

        NavHost(
            navController = navController,
            startDestination = "StartScreen",
            modifier = Modifier
                .fillMaxSize()
                .padding(innerPadding)){
            composable(route = "StartScreen"){
                if(uiState.previousScreen.size >= navController.currentBackStack.value.size){
                    viewModel.updateAppBarTextBacktrack()
                }
                StartScreen(
                    onArtistButtonClick =
                    {
                        viewModel.updatePreviousScreen(R.string.choose_artists)
                        viewModel.updateAppBarText()
                        navController.navigate("ArtistOrCategoryScreen")},
                    onCategoryButtonClick = {
                        viewModel.updatePreviousScreen(R.string.choose_category)
                        viewModel.updateAppBarText()
                        navController.navigate("ArtistOrCategoryScreen")},
                    onPaymentButtonClick = {
                        viewModel.updatePreviousScreen(R.string.payment_text)
                        viewModel.updateAppBarText()
                        navController.navigate("toPayment")},
                    viewModel = viewModel,
                    cart = uiState.shoppingCartItems,
                    totalPrice = uiState.totalPrice,
                    )
            }

            composable(route = "ArtistOrCategoryScreen"){

                val data: List<ChoiceListing> = when(uiState.currentScreen){
                    R.string.choose_artists -> {
                        ArtForSale.buildListing("artist")
                    }
                    else -> {
                        ArtForSale.buildListing("category")
                    }
                }

                ArtistOrCategoryScreen(
                    list = data,
                    viewModel = viewModel,
                    navController = navController
                    )

        }
            composable(route = "listPhotos"){
                if(uiState.previousScreen.size >= navController.currentBackStack.value.size){
                    viewModel.updateAppBarTextBacktrack()
                }

                val data = ArtForSale.getPhotos(uiState.currentScreen)
                GridPhotosScreen(
                    photoList = data,
                    viewModel = viewModel,
                    navController = navController)

            }

            composable(route = "selectedPhoto"){

                val photo = ArtForSale.listOfArt.filter{key -> key.title == uiState.currentScreen}
                if(photo.isNotEmpty()) {
                    SelectedPhotoScreen(
                        photo = photo[0],
                        viewModel = viewModel,
                        navController = navController
                    )
                }

            }

            composable(route = "toPayment"){

                PaymentScreen(
                    shoppingCart = uiState.shoppingCartItems,
                    totalPrice = uiState.totalPrice,
                    onClick = {viewModel.resetOrder()
                    navController.popBackStack("StartScreen", inclusive = false)})

            }

    }

}}