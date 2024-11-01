package com.example.kunsthandler.model

import androidx.annotation.DrawableRes
import com.example.kunsthandler.R

enum class CategoryType(@DrawableRes val title: Int, @DrawableRes val photo: Int ){
    NATURE(title = R.string.category_nature,photo = R.drawable.baseline_landscape_24 ),
    ARCHITECTURE(title = R.string.category_architecture,photo = R.drawable.baseline_apartment_24),
    MISCELLANEOUS(title = R.string.category_miscellaneous,photo = R.drawable.baseline_widgets_24),
    ANIMALS(title = R.string.category_animal,photo = R.drawable.baseline_pets_24),
    FOOD(title = R.string.category_food,photo = R.drawable.baseline_restaurant_24),
    PERSON(title = R.string.category_person,photo = R.drawable.baseline_person_24)
}
