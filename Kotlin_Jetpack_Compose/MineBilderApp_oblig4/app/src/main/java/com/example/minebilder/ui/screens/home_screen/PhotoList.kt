package com.example.minebilder.ui.screens.home_screen

import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.MaterialTheme
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import com.example.minebilder.model.DownloadedPhoto
import com.example.minebilder.model.SavedPhoto

@Composable
fun PhotoList(
    photos: List<Any>,
    onPhotoClick: (Any) -> Unit,
    onSaveClick: (DownloadedPhoto) -> Unit,
    onDeleteClick: (SavedPhoto) -> Unit
) {
    LazyColumn {
        items(photos) { photo ->
            Card(modifier = Modifier
                .padding(vertical = 8.dp, horizontal = 8.dp)
                .fillMaxWidth(),
                elevation = CardDefaults.cardElevation(defaultElevation = 4.dp),
                shape = RoundedCornerShape(0),
                colors = CardDefaults.cardColors(containerColor = MaterialTheme.colorScheme.primary)) {
                when (photo) {
                    is DownloadedPhoto -> DownloadedPhotoListItem(photo, { onPhotoClick(photo) }, onSaveClick)
                    is SavedPhoto -> SavedPhotoListItem(photo, { onPhotoClick(photo) }, onDeleteClick)
                }
            }
        }
    }
}