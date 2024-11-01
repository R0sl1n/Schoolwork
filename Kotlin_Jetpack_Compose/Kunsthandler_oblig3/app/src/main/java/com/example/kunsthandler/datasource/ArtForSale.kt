package com.example.kunsthandler.datasource

import android.annotation.SuppressLint
import androidx.annotation.DrawableRes
import com.example.kunsthandler.R
import com.example.kunsthandler.model.Artist
import com.example.kunsthandler.model.CategoryType
import com.example.kunsthandler.model.Photo

// Used for holding data about category or artist.
data class ChoiceListing(
    @DrawableRes val name: Int,
    @DrawableRes val photo: Int,
    val numOfPhotos: Int,
    val mostExpensivePhoto: Double,
    val mostPopular: Int
)

object ArtForSale {

    private val mapOfArtists = mapOf(
        "Tine Knutsen" to Artist(1,R.string.artist_name_tine_knutsen,4,R.string.art_25_name,R.drawable.baseline_person_24),
        "Jørn Nilsen" to Artist(2,R.string.artist_name_jørn_nilsen,4,R.string.art_18_name,R.drawable.baseline_person_24),
        "Mats Sørli" to Artist(3,R.string.artist_name_mats_sørli,4,R.string.art_19_name,R.drawable.baseline_person_24),
        "Lea Nyland" to Artist(4,R.string.artist_name_lea_nyland,4,R.string.art_4_name,R.drawable.baseline_person_24),
        "Ine Lauritsen" to Artist(5,R.string.artist_name_ine_lauritsen,4,R.string.art_21_name,R.drawable.baseline_person_24),
        "Andre Gamst" to Artist(6,R.string.artist_name_andre_gamst,4,R.string.art_14_name,R.drawable.baseline_person_24),
        "Nikolai Grønning" to Artist(7,R.string.artist_name_nikolai_grønning,4,R.string.art_15_name,R.drawable.baseline_person_24),
        "Ingvild Bakkehaug" to Artist(8,R.string.artist_name_ingvild_bakkehaug,4,R.string.art_24_name,R.drawable.baseline_person_24)
        )



    val listOfArt = listOf(
        Photo(
            id = 1,
            title = R.string.art_1_name,
            imageRes = R.drawable.biology,
            artist = mapOfArtists["Tine Knutsen"],
            category = CategoryType.ANIMALS,
            price = 145.45
        ),
        Photo(
            id = 2,
            title = R.string.art_2_name,
            imageRes = R.drawable.gaming,
            artist = mapOfArtists["Jørn Nilsen"],
            category = CategoryType.MISCELLANEOUS,
            price = 134.23
        ),
        Photo(
            id = 3,
            title = R.string.art_3_name,
            imageRes = R.drawable.drawing,
            artist = mapOfArtists["Mats Sørli"],
            category = CategoryType.NATURE,
            price = 266.43
        ), Photo(
            id = 4,
            title = R.string.art_4_name,
            imageRes = R.drawable.image1,
            artist = mapOfArtists["Lea Nyland"],
            category = CategoryType.NATURE,
            price = 123.54
        ), Photo(
            id = 5,
            title = R.string.art_5_name,
            imageRes = R.drawable.automotive,
            artist = mapOfArtists["Ine Lauritsen"],
            category = CategoryType.MISCELLANEOUS,
            price = 345.45
        ), Photo(
            id = 6,
            title = R.string.art_6_name,
            imageRes = R.drawable.image10,
            artist = mapOfArtists["Andre Gamst"],
            category = CategoryType.NATURE,
            price = 345.45
        ), Photo(
            id = 7,
            title = R.string.art_7_name,
            imageRes = R.drawable.image3,
            artist = mapOfArtists["Nikolai Grønning"],
            category = CategoryType.NATURE,
            price = 212.12
        ), Photo(
            id = 8,
            title = R.string.art_8_name,
            imageRes = R.drawable.culinary,
            artist = mapOfArtists["Ingvild Bakkehaug"],
            category = CategoryType.FOOD,
            price = 234.76
        ),
        Photo(
            id = 9,
            title = R.string.art_9_name,
            imageRes = R.drawable.crafts,
            artist = mapOfArtists["Tine Knutsen"],
            category = CategoryType.ANIMALS,
            price = 512.63
        ),
        Photo(
            id = 10,
            title = R.string.art_10_name,
            imageRes = R.drawable.architecture,
            artist = mapOfArtists["Jørn Nilsen"],
            category = CategoryType.ARCHITECTURE,
            price = 274.37
        ),
        Photo(
            id = 11,
            title = R.string.art_11_name,
            imageRes = R.drawable.image4,
            artist = mapOfArtists["Mats Sørli"],
            category = CategoryType.NATURE,
            price = 312.19
        ), Photo(
            id = 12,
            title = R.string.art_12_name,
            imageRes = R.drawable.image6,
            artist = mapOfArtists["Lea Nyland"],
            category = CategoryType.NATURE,
            price = 621.57
        ), Photo(
            id = 13,
            title = R.string.art_13_name,
            imageRes = R.drawable.image7,
            artist = mapOfArtists["Ine Lauritsen"],
            category = CategoryType.NATURE,
            price = 234.54
        ), Photo(
            id = 14,
            title = R.string.art_14_name,
            imageRes = R.drawable.image8,
            artist = mapOfArtists["Andre Gamst"],
            category = CategoryType.MISCELLANEOUS,
            price = 331.12
        ), Photo(
            id = 15,
            title = R.string.art_15_name,
            imageRes = R.drawable.image9,
            artist = mapOfArtists["Nikolai Grønning"],
            category = CategoryType.NATURE,
            price = 187.46
        ), Photo(
            id = 16,
            title = R.string.art_16_name,
            imageRes = R.drawable.design,
            artist = mapOfArtists["Ingvild Bakkehaug"],
            category = CategoryType.MISCELLANEOUS,
            price = 325.45
        ),
        Photo(
            id = 17,
            title = R.string.art_17_name,
            imageRes = R.drawable.fashion,
            artist = mapOfArtists["Tine Knutsen"],
            category = CategoryType.MISCELLANEOUS,
            price = 296.92
        ),
        Photo(
            id = 18,
            title = R.string.art_18_name,
            imageRes = R.drawable.ecology,
            artist = mapOfArtists["Jørn Nilsen"],
            category = CategoryType.NATURE,
            price = 134.23
        ),
        Photo(
            id = 19,
            title = R.string.art_19_name,
            imageRes = R.drawable.history,
            artist = mapOfArtists["Mats Sørli"],
            category = CategoryType.ARCHITECTURE,
            price = 266.43
        ), Photo(
            id = 20,
            title = R.string.art_20_name,
            imageRes = R.drawable.engineering,
            artist = mapOfArtists["Lea Nyland"],
            category = CategoryType.ARCHITECTURE,
            price = 834.34
        ), Photo(
            id = 21,
            title = R.string.art_21_name,
            imageRes = R.drawable.film,
            artist = mapOfArtists["Ine Lauritsen"],
            category = CategoryType.PERSON,
            price = 127.45
        ), Photo(
            id = 22,
            title = R.string.art_22_name,
            imageRes = R.drawable.finance,
            artist = mapOfArtists["Andre Gamst"],
            category = CategoryType.ARCHITECTURE,
            price = 416.73
        ), Photo(
            id = 23,
            title = R.string.art_23_name,
            imageRes = R.drawable.geology,
            artist = mapOfArtists["Nikolai Grønning"],
            category = CategoryType.NATURE,
            price = 265.49
        ), Photo(
            id = 24,
            title = R.string.art_24_name,
            imageRes = R.drawable.music,
            artist = mapOfArtists["Ingvild Bakkehaug"],
            category = CategoryType.PERSON,
            price = 721.21
        ),
        Photo(
            id = 25,
            title = R.string.art_25_name,
            imageRes = R.drawable.tech,
            artist = mapOfArtists["Tine Knutsen"],
            category = CategoryType.MISCELLANEOUS,
            price = 461.12
        ),
        Photo(
            id = 26,
            title = R.string.art_26_name,
            imageRes = R.drawable.physics,
            artist = mapOfArtists["Jørn Nilsen"],
            category = CategoryType.MISCELLANEOUS,
            price = 187.12
        ),
        Photo(
            id = 27,
            title = R.string.art_27_name,
            imageRes = R.drawable.photography,
            artist = mapOfArtists["Mats Sørli"],
            category = CategoryType.PERSON,
            price = 517.23
        ), Photo(
            id = 28,
            title = R.string.art_28_name,
            imageRes = R.drawable.painting,
            artist = mapOfArtists["Lea Nyland"],
            category = CategoryType.NATURE,
            price = 385.74
        ), Photo(
            id = 29,
            title = R.string.art_29_name,
            imageRes = R.drawable.lifestyle,
            artist = mapOfArtists["Ine Lauritsen"],
            category = CategoryType.PERSON,
            price = 195.45
        ), Photo(
            id = 30,
            title = R.string.art_30_name,
            imageRes = R.drawable.law,
            artist = mapOfArtists["Andre Gamst"],
            category = CategoryType.ARCHITECTURE,
            price = 278.74
        ), Photo(
            id = 31,
            title = R.string.art_31_name,
            imageRes = R.drawable.image2,
            artist = mapOfArtists["Nikolai Grønning"],
            category = CategoryType.PERSON,
            price = 345.45
        ), Photo(
            id = 32,
            title = R.string.art_32_name,
            imageRes = R.drawable.business,
            artist = mapOfArtists["Ingvild Bakkehaug"],
            category = CategoryType.ARCHITECTURE,
            price = 215.65
        )

    )
    // Used for creating the data for artistOrCategory screen.
    @SuppressLint("ResourceType")
    fun buildListing(type: String): MutableList<ChoiceListing>{
        val list = mutableListOf<ChoiceListing>()
        if(type == "artist"){
            for(artist in mapOfArtists.values) {
                val mostExpensive = listOfArt.filter { it.artist?.id == artist.id }.maxOf { it.price }
                val numberOfPhotos = listOfArt.filter { it.artist?.id == artist.id }.size
                val mostPopular = artist.mostPopularPhoto
                list.add(ChoiceListing(
                    name = artist.name,
                    photo = artist.photo,
                    numOfPhotos = numberOfPhotos,
                    mostExpensivePhoto = mostExpensive,
                    mostPopular = mostPopular))
            }
        }
        else {
            for(category in CategoryType.entries){
                val mostExpensive = listOfArt.filter { it.category == category}.toList().maxOf { it.price }
                val numberOfPhotos = listOfArt.filter { it.category == category }.size
                val mostPopular = listOfArt.filter { it.category == category }.map{it.title}[0]
                list.add(
                    ChoiceListing(
                    name = category.title,
                        photo = category.photo,
                        numOfPhotos = numberOfPhotos,
                        mostExpensivePhoto = mostExpensive,
                        mostPopular = mostPopular))

            }
            }
        return list
    }

    fun getPhotos(typeId: Int): List<Photo>{
        val list = CategoryType.entries.map{it.title}
        return if(typeId in list){
            listOfArt.filter { it.category.title == typeId }
        } else listOfArt.filter { it.artist?.name ==typeId }
    }

}