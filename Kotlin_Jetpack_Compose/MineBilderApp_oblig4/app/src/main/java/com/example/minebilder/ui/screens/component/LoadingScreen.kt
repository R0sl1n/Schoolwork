package com.example.minebilder.ui.screens.component

import android.content.res.Configuration
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.LinearProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.example.minebilder.R
import com.example.minebilder.ui.MineBilderViewModel
import com.example.minebilder.ui.theme.MineBilderTheme
@Composable
fun LoadingScreen(viewModel: MineBilderViewModel){
    val currentDownloaded = viewModel.photoUiState.collectAsState().value.downloadedPhotoList.size
    StatusIndicator(currentDownloaded)
}

@Composable
fun StatusIndicator(currentDownloaded: Int){
    Column(modifier = Modifier.padding(horizontal = 16.dp),
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally) {

        LinearProgressIndicator(
            progress =  currentDownloaded/5000F,
            modifier = Modifier
                .fillMaxWidth()
                .height(19.dp),
            color = MaterialTheme.colorScheme.primary,
        )

        Row(
            modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.SpaceBetween
        ) {
            Text(
                text = stringResource(R.string.photo_percentage, (currentDownloaded/5000.0*100).toInt()),
                textAlign = TextAlign.Start,
                modifier = Modifier.weight(1f)
            )
            Text(
                text = "100%",
                textAlign = TextAlign.End,
                modifier = Modifier.weight(1f)
            )
        }
        Text(
            stringResource(R.string.loading_images_message),
            style = MaterialTheme.typography.bodyLarge,
            color = MaterialTheme.colorScheme.onBackground
        )
    }

}

@Preview(showBackground = true, showSystemUi = true)
@Composable
fun LoadingScreenLightModePreview(){
    MineBilderTheme {
        Surface(
            modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background
        ) {
            StatusIndicator(2500)
    }}
}

@Preview(showBackground = true, showSystemUi = true, uiMode = Configuration.UI_MODE_NIGHT_YES)
@Composable
fun LoadingScreenDarkModePreview(){
    MineBilderTheme {
        Surface(
            modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background
        ) {
            StatusIndicator(2500)
        }}
}
//Gammel Loading slett før levering
/*
//LoadingScreen is from MarsPhotos code lab
// https://github.com/google-developer-training/basic-android-kotlin-compose-training-mars-photos
@Composable
fun LoadingScreen(){
    Column(verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally) {
        Image(
            modifier = Modifier.size(200.dp),
            painter = painterResource(R.drawable.loading_img),
            contentDescription = stringResource(R.string.loading)
        )
        Text(
            stringResource(R.string.loading_images_message),
            style = MaterialTheme.typography.bodyLarge,
            color = MaterialTheme.colorScheme.onBackground
        )
    }

}
 */