package com.example.kunsthandler

import androidx.activity.ComponentActivity
import androidx.compose.ui.test.junit4.createAndroidComposeRule
import androidx.compose.ui.test.assertIsDisplayed
import androidx.compose.ui.test.onNodeWithContentDescription
import androidx.compose.ui.test.onNodeWithText
import androidx.compose.ui.test.performClick
import com.example.kunsthandler.ui.theme.KunstHandelTheme
import org.junit.Rule
import org.junit.Test
import com.example.kunsthandler.datasource.ArtForSale
import junit.framework.TestCase.fail

class KunstHandelUITest {

    @get:Rule
    val composeTestRule = createAndroidComposeRule<ComponentActivity>()

    @Test
    fun testAddingItemWithArtistAndFrameSelectionToShoppingCart() {

        composeTestRule.setContent {

            KunstHandelTheme {
                KunstHandelApp()
            }
        }

        // Navigate to the artist selection screen.
        composeTestRule.onNodeWithText("Kunstner").performClick()

        // Select the first artist in the list. Assuming artists are listed by name.
        composeTestRule.onNodeWithText("Tine Knutsen").performClick()

        // Select the first picture in the list. Assuming pictures are listed by name.
        composeTestRule.onNodeWithText("Sjøhest").performClick()

        // Select frame options.
        // Using the visible text next to the radio button to find and select the correct option.
        composeTestRule.onNodeWithText("Tre (120 kr)").performClick()
        // Select the "Medium (120 kr)" size option.
        composeTestRule.onNodeWithText("Medium (120 kr)").performClick()
        // Select the "15 (70 kr)" width option.
        composeTestRule.onNodeWithText("15 (70 kr)").performClick()

        // Add the item with selected frame options to the shopping cart.
        // Use the "Legg i handlekurv" button text to find and click the add to cart button.
        composeTestRule.onNodeWithText("Legg i handlekurv").performClick()

        // Verify the number of pictures in the cart. This assumes that the text updates upon adding an item.
        // "Antall valgte bilder: 1" should be visible on the screen.
        composeTestRule.onNodeWithText("Antall valgte bilder: 1").assertIsDisplayed()


    }
    @Test
    fun testRemovingItemFromShoppingCart() {
        composeTestRule.setContent {
            KunstHandelTheme {
                KunstHandelApp()
            }
        }

        // Navigate to the artist selection screen.
        composeTestRule.onNodeWithText("Kunstner").performClick()

        // Select the artist in the list. Assuming artists are listed by name.
        composeTestRule.onNodeWithText("Andre Gamst").performClick()

        // Select the first picture in the list.
        composeTestRule.onNodeWithText("Salt").performClick()

        // Select frame options.
        // Using the visible text next to the radio button to find and select the correct option.
        composeTestRule.onNodeWithText("Tre (120 kr)").performClick()
        // Select the "Medium (120 kr)" size option.
        composeTestRule.onNodeWithText("Medium (120 kr)").performClick()
        // Select the "15 (70 kr)" width option.
        composeTestRule.onNodeWithText("15 (70 kr)").performClick()

        // Add the item with selected frame options to the shopping cart.
        // Use the "Legg i handlekurv" button text to find and click the add to cart button.
        composeTestRule.onNodeWithText("Legg i handlekurv").performClick()

        // Verify the number of pictures in the cart.
        // "Antall valgte bilder: 1" should be visible on the screen.
        composeTestRule.onNodeWithText("Antall valgte bilder: 1").assertIsDisplayed()

        // Remove the first item in the cart.

        composeTestRule.onNodeWithText("Slett", useUnmergedTree = true).performClick()

        // Alternatively, if the cart shows an updated count of items, use that for verification.
        composeTestRule.onNodeWithText("Antall valgte bilder: 0").assertIsDisplayed()
    }


    @Test
    fun testViewArtistDetailAndReturn() {
        composeTestRule.setContent {
            KunstHandelTheme {
                KunstHandelApp()
            }
        }
        // View art from a specific artist
        composeTestRule.onNodeWithText("Kunstner").performClick()
        composeTestRule.onNodeWithText("Mats Sørli").performClick()

        // Verify that on a specific artists page.
        composeTestRule.onNodeWithText("Mats Sørli").assertIsDisplayed()

        // Go back to the artist selection screen.
        composeTestRule.onNodeWithContentDescription("Tilbake").performClick()

    }

    @Test
    fun testViewCategoryDiverse() {
        composeTestRule.setContent {
            KunstHandelTheme {
                KunstHandelApp()
            }
        }
        composeTestRule.onNodeWithText("Kategori").performClick()
        composeTestRule.onNodeWithText("Diverse").performClick()
        composeTestRule.onNodeWithText("Sjokk").assertIsDisplayed()

    }
    @Test
    fun testMultipleImagesInAllCategoriesAndClickMostPopulatedCategory() {
        composeTestRule.setContent {
            KunstHandelTheme {
                KunstHandelApp()
            }
        }
        composeTestRule.onNodeWithText("Kategori").performClick()

        // Group photos by category and count them
        val categoryPhotoCounts = ArtForSale.listOfArt.groupBy { it.category }.mapValues { it.value.size }

        // Find the category with the most photos
        val mostPopulatedCategory = categoryPhotoCounts.maxByOrNull { it.value }?.key

        // Check that a category was indeed found
        if (mostPopulatedCategory == null) {
            fail("Failed to identify the category with the most photos.")
            return
        }

        // Use the resource ID for the category name to get the actual string name of the category
        val categoryName = composeTestRule.activity.resources.getString(mostPopulatedCategory.title)

        // Perform click action on the category with the most images
        composeTestRule.onNodeWithText(categoryName, useUnmergedTree = true).performClick()

    }
}

