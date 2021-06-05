package via.sep4.Diary;

import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;

import java.text.SimpleDateFormat;
import java.util.Calendar;

import via.sep4.R;
/**
 * @author Bogdan Mezei
 * @version 1.0
 */
public class addDiaryEntry extends Fragment
{
	addDiaryEntryViewModel viewModel;
	TextView entry;
	TextView date;
	Button save;
	Button delete;
	public addDiaryEntry()
	{
		// Required empty public constructor
	}
	
	@Override
	public void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
	}
	
	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
	{
		// Inflate the layout for this fragment
		View root = inflater.inflate(R.layout.fragment_add_diary_entry, container, false);
		viewModel = new ViewModelProvider(this).get(addDiaryEntryViewModel.class);
		entry = root.findViewById(R.id.entry);
		date = root.findViewById(R.id.date);
		save = root.findViewById(R.id.savebutton);
		delete = root.findViewById(R.id.delete);
		save.setOnClickListener(new View.OnClickListener()
		{
			@Override
			public void onClick(View v)
			{
				DiaryEntry toAdd = new DiaryEntry(entry.getText().toString(), Calendar.getInstance().getTime());
				viewModel.insert(toAdd);
				getActivity().onBackPressed();
			}
		});
		if(getArguments() != null)
		{
			DiaryEntry toEdit = (DiaryEntry) getArguments().getSerializable("toEdit");
			SimpleDateFormat format = new SimpleDateFormat("dd/MM/yyyy, hh:mm a");
			date.setText("Date added: "+format.format(toEdit.getDateAdded()));
			entry.setText(toEdit.getEntry());
			entry.setFocusable(false);
			save.setVisibility(View.GONE);
			delete.setOnClickListener(new View.OnClickListener()
			{
				@Override
				public void onClick(View v)
				{
					viewModel.delete(toEdit);
					getActivity().onBackPressed();
				}
			});
		}
		else
		{
			date.setVisibility(View.GONE);
			delete.setVisibility(View.GONE);
		}
		return root;
	}
}