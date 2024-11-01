package com.example.minebilder.ui.screens.home_screen

import android.content.res.Configuration
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import coil.compose.rememberAsyncImagePainter
import com.example.minebilder.R
import com.example.minebilder.model.DownloadedPhoto
import com.example.minebilder.model.SavedPhoto
import com.example.minebilder.ui.theme.MineBilderTheme

@Composable
fun DownloadedPhotoListItem(
    photo: DownloadedPhoto,
    onPhotoClick: () -> Unit,
    onSaveClick: (DownloadedPhoto) -> Unit
) {
    Row(verticalAlignment = Alignment.CenterVertically, modifier = Modifier.fillMaxWidth()) {
        Image(
            painter = rememberAsyncImagePainter(model = photo.thumbnailUrl,placeholder = painterResource(
                id = R.drawable.ic_launcher_foreground)),
            contentDescription = "Thumbnail",
            modifier = Modifier
                .size(80.dp)
                .clip(RoundedCornerShape(0))
        )
        Column(modifier = Modifier.padding(start = 8.dp)) {
            Text(photo.title,
                maxLines = 1,
                overflow = TextOverflow.Ellipsis,
                style = MaterialTheme.typography.bodyLarge,
                color = MaterialTheme.colorScheme.onPrimary)
            Row {
                TextButton(onClick = onPhotoClick,
                    colors = ButtonDefaults.buttonColors(MaterialTheme.colorScheme.primaryContainer),
                    shape = RoundedCornerShape(0),
                    contentPadding = PaddingValues(0.dp)
                ) {
                    Text(
                        stringResource(R.string.show),
                        style = MaterialTheme.typography.labelMedium,
                        color = MaterialTheme.colorScheme.onPrimaryContainer)
                }
                Spacer(modifier = Modifier.padding(end = 5.dp))
                TextButton(
                    onClick = { onSaveClick(photo) },
                    colors = ButtonDefaults.buttonColors(MaterialTheme.colorScheme.primaryContainer),
                    shape = RoundedCornerShape(0),
                ) {
                    Text(
                        stringResource(R.string.save),
                        style = MaterialTheme.typography.labelMedium,
                        color = MaterialTheme.colorScheme.onPrimaryContainer)
                }
            }
        }
    }
}

@Composable
fun SavedPhotoListItem(
    photo: SavedPhoto,
    onPhotoClick: () -> Unit,
    onDeleteClick: (SavedPhoto) -> Unit
) {
    Row(verticalAlignment = Alignment.CenterVertically, modifier = Modifier.fillMaxWidth()) {
        Image(
            painter = rememberAsyncImagePainter(model = photo.thumbnailUrl, placeholder = painterResource(
                id = R.drawable.ic_launcher_foreground)),
            contentDescription = "Thumbnail",
            modifier = Modifier
                .size(80.dp)
                .clip(RoundedCornerShape(4.dp))
        )
        Column(modifier = Modifier.padding(start = 8.dp)) {
            Text(
                photo.title,
                maxLines = 1,
                overflow = TextOverflow.Ellipsis,
                style = MaterialTheme.typography.bodyLarge,
                color = MaterialTheme.colorScheme.onPrimary)
            Row {
                TextButton(onClick = onPhotoClick,
                    colors = ButtonDefaults.buttonColors(MaterialTheme.colorScheme.primaryContainer),
                    shape = RoundedCornerShape(0)
                ) {
                    Text(
                        stringResource(R.string.show),
                        style = MaterialTheme.typography.labelMedium,
                        color = MaterialTheme.colorScheme.onPrimaryContainer)
                }
                Spacer(modifier = Modifier.padding(end = 5.dp))
                TextButton(onClick = { onDeleteClick(photo) },
                    colors = ButtonDefaults.buttonColors(MaterialTheme.colorScheme.primaryContainer),
                    shape = RoundedCornerShape(0)
                ) {
                    Text(
                        stringResource(R.string.delete),
                        style = MaterialTheme.typography.labelMedium,
                        color = MaterialTheme.colorScheme.onPrimaryContainer)
                }
            }
        }
    }
}

@Preview(showBackground = true)
@Composable
fun DownloadedPhotoListItemLightModePreview(){
    val mockedDownloadPhoto =  DownloadedPhoto(0, 0,"Fake title","","")
    MineBilderTheme {
        Surface(color = MaterialTheme.colorScheme.background) {
        Card(modifier = Modifier
            .padding(vertical = 8.dp, horizontal = 8.dp)
            .fillMaxWidth(),
            elevation = CardDefaults.cardElevation(defaultElevation = 4.dp),
            shape = RoundedCornerShape(0),
            colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.primary)) {
        DownloadedPhotoListItem(photo = mockedDownloadPhoto, onPhotoClick ={} , onSaveClick ={} )
    }}}
}

@Preview(showBackground = true)
@Composable
fun SavedPhotoListItemLightModePreview(){
    val mockedDownloadPhoto =  SavedPhoto(0, 0,"Fake title","","")
    MineBilderTheme {
        Surface(color = MaterialTheme.colorScheme.background) {
        Card(modifier = Modifier
            .padding(vertical = 8.dp, horizontal = 8.dp)
            .fillMaxWidth(),
            elevation = CardDefaults.cardElevation(defaultElevation = 4.dp),
            shape = RoundedCornerShape(0),
            colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.primary)) {
        SavedPhotoListItem(photo = mockedDownloadPhoto, onPhotoClick ={} , onDeleteClick ={} )
    }}}
}

@Preview(showBackground = true, uiMode = Configuration.UI_MODE_NIGHT_YES)
@Composable
fun DownloadedPhotoListItemDarkModePreview(){
    val mockedDownloadPhoto =  DownloadedPhoto(0, 0,"Fake title","","")
    MineBilderTheme {
        Surface(color = MaterialTheme.colorScheme.background) {
        Card(modifier = Modifier
            .padding(vertical = 8.dp, horizontal = 8.dp)
            .fillMaxWidth(),
            elevation = CardDefaults.cardElevation(defaultElevation = 4.dp),
            shape = RoundedCornerShape(0),
            colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.primary)) {
            DownloadedPhotoListItem(photo = mockedDownloadPhoto, onPhotoClick ={} , onSaveClick ={} )
        }}}
}

@Preview(showBackground = true, uiMode = Configuration.UI_MODE_NIGHT_YES)
@Composable
fun SavedPhotoListItemDarkModePreview(){
    val mockedDownloadPhoto =  SavedPhoto(0, 0,"Fake title","","")
    MineBilderTheme {

        Surface(color = MaterialTheme.colorScheme.background) {
        Card(modifier = Modifier
            .padding(vertical = 8.dp, horizontal = 8.dp)
            .fillMaxWidth(),
            elevation = CardDefaults.cardElevation(defaultElevation = 4.dp),
            shape = RoundedCornerShape(0),
            colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.primary)) {
            SavedPhotoListItem(photo = mockedDownloadPhoto, onPhotoClick ={} , onDeleteClick ={} )
        }}}
}