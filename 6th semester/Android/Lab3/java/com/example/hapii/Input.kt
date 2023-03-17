package com.example.hapii
import android.os.Parcelable
import kotlinx.android.parcel.Parcelize

@Parcelize
data class Input(
    val text: String,
    val font: Int,
) : Parcelable