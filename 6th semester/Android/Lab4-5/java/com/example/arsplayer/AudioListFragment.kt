package com.example.arsplayer

import android.content.ContentUris
import android.content.Intent
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import android.provider.MediaStore
import android.net.Uri
import android.os.Build
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView


class AudioListFragment : Fragment() {
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_audio_list, container, false)

        val list = getAudioFiles()

        val audioList = view.findViewById<RecyclerView>(R.id.audio_list)
        val linkButton = view.findViewById<Button>(R.id.button)
        val linkText = view.findViewById<EditText>(R.id.linkText)
        audioList.layoutManager = LinearLayoutManager(view.context)
        audioList.adapter = AudioListAdapter(list) { audio -> adapterOnClick(audio) }

        linkButton.setOnClickListener{
            val internet = linkText.text.toString()
            lateinit var uri : Uri
            if (internet.contains("https://")) {
                uri = Uri.parse(internet)
                adapterOnClick(AudioFile(internet, internet, internet, 0, uri))
            } else {
                Toast.makeText(
                    this.context,
                    "Wrong Link",
                    Toast.LENGTH_LONG
                ).show()
            }
        }
        return view
    }

    private fun getAudioFiles(): ArrayList<AudioFile> {
        val audioList = arrayListOf<AudioFile>()
        val collection =
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q) {
                MediaStore.Audio.Media.getContentUri(
                    MediaStore.VOLUME_EXTERNAL
                )
            } else {
                MediaStore.Audio.Media.EXTERNAL_CONTENT_URI
            }

        val projection = arrayOf(
            MediaStore.Audio.Media._ID,
            MediaStore.Audio.Media.TITLE,
            MediaStore.Audio.Media.ARTIST,
            MediaStore.Audio.Media.DURATION,
            MediaStore.Audio.Media.DISPLAY_NAME,
        )

        val query = context?.contentResolver?.query(
            collection,
            projection,
            null,
            null,
            null
        )
        query?.use { cursor ->
            val idColumn = cursor.getColumnIndexOrThrow(MediaStore.Audio.Media._ID)
            val titleColumn =
                cursor.getColumnIndexOrThrow(MediaStore.Audio.Media.TITLE)
            val artistColumn =
                cursor.getColumnIndexOrThrow(MediaStore.Audio.Media.ARTIST)
            val durationColumn =
                cursor.getColumnIndexOrThrow(MediaStore.Audio.Media.DURATION)

            val column = cursor.getColumnIndexOrThrow(MediaStore.Audio.Media.DISPLAY_NAME)

            while (cursor.moveToNext()) {
                val id = cursor.getLong(idColumn)
                val title = cursor.getString(titleColumn)
                val artist = cursor.getString(artistColumn)
                val duration = cursor.getInt(durationColumn)

                val name = cursor.getString(column)

                val contentUri: Uri = ContentUris.withAppendedId(
                    MediaStore.Audio.Media.EXTERNAL_CONTENT_URI,
                    id
                )
                audioList += AudioFile(title, artist, name, duration, contentUri)
            }
        }

        return audioList
    }

    private fun adapterOnClick(audio: AudioFile) {
        val intent = Intent(this.context, PlaySongActivity()::class.java)
        intent.putExtra("audioFile", audio)
        startActivity(intent)
    }

}