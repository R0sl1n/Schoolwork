package com.example.minebilder.ui.screens.component

import android.content.res.Configuration
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.tooling.preview.Preview
import com.example.minebilder.R
import com.example.minebilder.ui.theme.MineBilderTheme

//ErrorScreen is from MarsPhotos code lab
// https://github.com/google-developer-training/basic-android-kotlin-compose-training-mars-photos
@Composable
fun ErrorScreen(
    onErrorClick: () -> Unit
){
    Column(
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Image(
            painter = painterResource(id = R.drawable.ic_connection_error), contentDescription = ""
        )
        Text(text = stringResource(R.string.failed_to_load_photos),
            style = MaterialTheme.typography.bodyLarge,
            color = MaterialTheme.colorScheme.onBackground)
        Button(onClick = onErrorClick,
            colors = ButtonDefaults.buttonColors(MaterialTheme.colorScheme.primaryContainer),
            shape = RoundedCornerShape(0)) {
            Text(stringResource(R.string.retry),
                style = MaterialTheme.typography.labelMedium,
                color = MaterialTheme.colorScheme.onPrimaryContainer)
        }
    }
    
}

@Preview(showSystemUi = true, showBackground = true)
@Composable
fun ErrorScreenLightModePreview(){
    MineBilderTheme {
        Surface(
            modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background
        ) {
            ErrorScreen({})
        }
    }
}

@Preview(showSystemUi = true, showBackground = true, uiMode = Configuration.UI_MODE_NIGHT_YES)
@Composable
fun ErrorScreenDarkModePreview(){
    MineBilderTheme {
        Surface(
            modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background
        ) {
            ErrorScreen({})
        }
    }
}