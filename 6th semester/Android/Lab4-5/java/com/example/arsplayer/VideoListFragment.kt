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


class VideoListFragment : Fragment() {
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_video_list, container, false)

        val list = getVideoFiles()

        val linkButton = view.findViewById<Button>(R.id.button)
        val linkText = view.findViewById<EditText>(R.id.linkText)
        val videoList = view.findViewById<RecyclerView>(R.id.video_list)
        videoList.layoutManager = LinearLayoutManager(view.context)
        videoList.adapter = VideoListAdapter(list) { video -> adapterOnClick(video) }

        linkButton.setOnClickListener {
            val internet = linkText.text.toString()
            lateinit var uri: Uri
            if (internet.contains("https://")) {
                uri = Uri.parse(internet)
                adapterOnClick(VideoFile(internet, internet, 0, 0, 0, uri))
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

    private fun getVideoFiles(): ArrayList<VideoFile> {
        val videoList = arrayListOf<VideoFile>()
        val collection =
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q) {
                MediaStore.Video.Media.getContentUri(
                    MediaStore.VOLUME_EXTERNAL
                )
            } else {
                MediaStore.Video.Media.EXTERNAL_CONTENT_URI
            }

        val projection = arrayOf(
            MediaStore.Video.Media._ID,
            MediaStore.Video.Media.TITLE,
            MediaStore.Video.Media.DURATION,
            MediaStore.Video.Media.WIDTH,
            MediaStore.Video.Media.HEIGHT,
            MediaStore.Video.Media.DISPLAY_NAME,
        )

        val query = context?.contentResolver?.query(
            collection,
            projection,
            null,
            null,
            null
        )
        query?.use { cursor ->
            val idColumn = cursor.getColumnIndexOrThrow(MediaStore.Video.Media._ID)
            val titleColumn =
                cursor.getColumnIndexOrThrow(MediaStore.Video.Media.TITLE)
            val durationColumn =
                cursor.getColumnIndexOrThrow(MediaStore.Video.Media.DURATION)
            val widthColumn =
                cursor.getColumnIndexOrThrow(MediaStore.Video.Media.WIDTH)
            val heightColumn =
                cursor.getColumnIndexOrThrow(MediaStore.Video.Media.HEIGHT)
            val column = cursor.getColumnIndexOrThrow(MediaStore.Video.Media.DISPLAY_NAME)

            while (cursor.moveToNext()) {
                val id = cursor.getLong(idColumn)
                val title = cursor.getString(titleColumn)
                val duration = cursor.getInt(durationColumn)
                val width = cursor.getInt(widthColumn)
                val height = cursor.getInt(heightColumn)

                val name = cursor.getString(column)

                val contentUri: Uri = ContentUris.withAppendedId(
                    MediaStore.Video.Media.EXTERNAL_CONTENT_URI,
                    id
                )
                videoList += VideoFile(title, name, duration, width, height, contentUri)
            }
        }

        return videoList
    }

    private fun adapterOnClick(video: VideoFile) {
        val intent = Intent(this.context, PlayVideoActivity()::class.java)
        intent.putExtra("videoFile", video)
        startActivity(intent)
    }
}