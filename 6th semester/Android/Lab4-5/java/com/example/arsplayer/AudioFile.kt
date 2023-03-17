package com.example.arsplayer

import android.net.Uri
import android.os.Parcelable
import kotlinx.android.parcel.Parcelize

@Parcelize
data class AudioFile(
    val title: String,
    val artist: String,
    val displayName: String,
    val duration: Int,
    val uri: Uri
) : Parcelable