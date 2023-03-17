package com.example.arsplayer

import android.net.Uri
import android.os.Parcelable
import kotlinx.android.parcel.Parcelize

@Parcelize
data class VideoFile(
    val title: String,
    val displayName: String,
    val duration: Int,
    val width: Int,
    val height: Int,
    val uri: Uri
) : Parcelable