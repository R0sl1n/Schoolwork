package com.example.minebilder.ui

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewModelScope
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import androidx.work.WorkInfo
import com.example.minebilder.MineBilderApplication
import com.example.minebilder.data.MineBilderRepository
import com.example.minebilder.model.DownloadedPhoto
import com.example.minebilder.model.SavedPhoto
import com.example.minebilder.model.toSavedPhoto
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.SharingStarted
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.combine
import kotlinx.coroutines.flow.stateIn
import kotlinx.coroutines.launch

//Holds the photos for the homescreen
data class PhotoUiState(
    val savedPhotoList: List<SavedPhoto> = listOf(),
    val downloadedPhotoList: List<DownloadedPhoto> = listOf()
)

//Used to decide which screen to display on app start.
sealed interface StatusUiState {
    data object Success : StatusUiState
    data object Error : StatusUiState
    data object Loading : StatusUiState
}

class MineBilderViewModel(
    val mineBilderRepository: MineBilderRepository

): ViewModel(){

    //Holding the status state for deciding which screen to display
    private val _uiStatusState = MutableStateFlow<StatusUiState>(StatusUiState.Loading)
    val uiStatusState: StateFlow<StatusUiState> = _uiStatusState.asStateFlow()

    //Combines the flow from the database queries into one stateflow.
    val photoUiState: StateFlow<PhotoUiState> = combine(mineBilderRepository.getAllDownloadedPhotosStream(),mineBilderRepository.getAllSavedPhotosStream()){
            downloaded,saved ->
        PhotoUiState(savedPhotoList = saved, downloadedPhotoList = downloaded)
    }.stateIn(
        scope = viewModelScope,
        started = SharingStarted.WhileSubscribed(5000L),
        initialValue = PhotoUiState()
    )

    init {
        isEmpty()
         getPhotosFromWeb()
    }


    private val _albumTitle = MutableStateFlow("")
    val albumTitle: StateFlow<String> = _albumTitle.asStateFlow()
    fun fetchAlbumTitle(albumId: Int) {
        viewModelScope.launch {
            mineBilderRepository.getAlbumTitle(albumId).collect { title: String ->  // Explicitly specifying the type
                _albumTitle.value = title
            }
        }
    }

    //Checks if database already contains downloaded photos
    private fun isEmpty(){
            viewModelScope.launch {
                when(mineBilderRepository.isEmpty()){
                    false -> _uiStatusState.value = StatusUiState.Success
                    else -> _uiStatusState.value = StatusUiState.Loading
                }
            }
    }

    //Call the WorkManager in the repository to start downloading photos from https://jsonplaceholder.typicode.com/posts
    fun getPhotosFromWeb(){

        viewModelScope.launch {
            mineBilderRepository.downloadPhotosFromSource().observeForever{ workInfo ->
                //Updates the uiStatusState if there is no photos already in database as WorkManager runs.
                if(_uiStatusState.value != StatusUiState.Success){
                when(workInfo.state){
                    WorkInfo.State.SUCCEEDED -> _uiStatusState.value = StatusUiState.Success
                    WorkInfo.State.FAILED -> _uiStatusState.value = StatusUiState.Error
                    else -> _uiStatusState.value = StatusUiState.Loading
                }}
            }
        }

    }
    fun savePhoto(photo: DownloadedPhoto){
        viewModelScope.launch{
            mineBilderRepository.insertToSaved(photo.toSavedPhoto())
        }
    }

    fun delete(photo: SavedPhoto){
        viewModelScope.launch { mineBilderRepository.deleteSavedPhoto(photo) }
    }


    companion object {
        val Factory: ViewModelProvider.Factory = viewModelFactory {
            initializer {
                val application = (this[ViewModelProvider.AndroidViewModelFactory.APPLICATION_KEY] as MineBilderApplication)
                val mineBilderRepository = application.container.mineBilderRepository
                MineBilderViewModel(mineBilderRepository = mineBilderRepository)
            }
        }
    }



}