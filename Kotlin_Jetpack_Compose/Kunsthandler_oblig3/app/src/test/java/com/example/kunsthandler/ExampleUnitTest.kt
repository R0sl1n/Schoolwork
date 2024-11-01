package com.example.kunsthandler

import org.junit.Before
import org.junit.Test
import org.junit.Assert.*
import com.example.kunsthandler.ui.KunstViewModel
import com.example.kunsthandler.model.Photo
import com.example.kunsthandler.model.CategoryType
import com.example.kunsthandler.model.Artist
import com.example.kunsthandler.datasource.ArtForSale


class KunstViewModelTest {

    private lateinit var viewModel: KunstViewModel

    @Before
    fun setupViewModel() {
        viewModel = KunstViewModel()
    }

    @Test
    fun updateShoppingCart_withPlasticFrameAndSmallSize() {
        val basePrice = ArtForSale.listOfArt.filter { it.id == 1 }[0].price
        val artist = Artist(1, R.string.artist_name_tine_knutsen, 10, R.string.art_25_name, R.drawable.image1)
        val photo = Photo(1, R.string.art_1_name, R.drawable.image1, artist, CategoryType.NATURE, basePrice)

        viewModel.updateShoppingCart(1, photo, R.string.frame_material_plastic, R.string.selected_photo_width_10, R.string.small)

        val expectedPrice = viewModel.formatPrice(basePrice + 78.0 + 50.0 + 70.0) // Based on prices defined in viewModel
        val actualPrice = viewModel.uiState.value.shoppingCartItems.first().totalPrice

        assertEquals("Total price does not match!", expectedPrice, actualPrice, 0.0)
    }

    @Test
    fun updateShoppingCart_withWoodFrameAndMediumSize() {
        val basePrice = ArtForSale.listOfArt.filter { it.id == 2 }[0].price
        val artist = Artist(2, R.string.artist_name_jørn_nilsen, 10, R.string.art_18_name, R.drawable.image2)
        val photo = Photo(2, R.string.art_2_name, R.drawable.image2, artist, CategoryType.ARCHITECTURE, basePrice)

        viewModel.updateShoppingCart(2, photo, R.string.frame_material_wood, R.string.selected_photo_width_15, R.string.medium)

        val expectedPrice = viewModel.formatPrice(basePrice + 120.0 + 70.0 + 120.0)
        val actualPrice = viewModel.uiState.value.shoppingCartItems.first().totalPrice

        assertEquals("Total price does not match!", expectedPrice, actualPrice, 0.0)
    }

    @Test
    fun updateShoppingCart_withMetalFrameAndLargeSize() {
        val basePrice = ArtForSale.listOfArt.filter { it.id == 3 }[0].price
        val artist = Artist(3, R.string.artist_name_mats_sørli, 10, R.string.art_19_name, R.drawable.image3)
        val photo = Photo(3, R.string.art_3_name, R.drawable.image3, artist, CategoryType.MISCELLANEOUS, basePrice)

        viewModel.updateShoppingCart(3, photo, R.string.frame_material_metal, R.string.selected_photo_width_20, R.string.large)

        val expectedPrice = viewModel.formatPrice(basePrice + 170.0 + 90.0 + 170.0)
        val actualPrice = viewModel.uiState.value.shoppingCartItems.first().totalPrice

        assertEquals("Total price does not match!", expectedPrice, actualPrice, 0.0)
    }
}
