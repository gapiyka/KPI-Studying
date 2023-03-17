package com.example.hapii

import android.content.Context
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TableLayout
import android.widget.TableRow
import android.widget.TextView
import androidx.fragment.app.Fragment

class ListFragment : Fragment() {
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        val view: View = inflater.inflate(R.layout.fragment_list, container, false)
        val data = this.activity?.intent?.getParcelableArrayListExtra<Input>("db")

        val fonts = listOf("Sans", "Mono", "Serif", "Nature")

        val table = view.findViewById<TableLayout>(R.id.table)
        for (item in data!!) {
            val row = TableRow(view.context)
            row.addView(createTextView(item.text, view.context))
            row.addView(createTextView(fonts[item.font], view.context))
            table.addView(row)
        }
        return view
    }

    private fun createTextView(string: String, context: Context): TextView {
        val text = TextView(context)
        text.text = string
        return text
    }

}