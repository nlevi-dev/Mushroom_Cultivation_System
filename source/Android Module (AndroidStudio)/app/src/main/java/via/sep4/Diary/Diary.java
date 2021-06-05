package via.sep4.Diary;

import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.google.android.material.floatingactionbutton.FloatingActionButton;

import java.util.ArrayList;

import via.sep4.R;
/**
 * @author Bogdan Mezei
 * @version 1.0
 */
public class Diary extends Fragment implements DiaryAdapter.OnListItemClickListener
{

    FloatingActionButton fab;
    private via.sep4.Diary.DiaryViewModel DiaryViewModel;
    RecyclerView mEntries;
    DiaryAdapter mEntryAdapter;
    public Diary() {
        // Required empty public constructor
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View root = inflater.inflate(R.layout.fragment_diary, container, false);
        mEntries = root.findViewById(R.id.rv);
        mEntries.hasFixedSize();
        mEntries.setLayoutManager(new LinearLayoutManager(this.getContext()));
        DiaryViewModel = new ViewModelProvider(this).get(DiaryViewModel.class);
    
    
        DiaryViewModel.getAllEntries().observe(getViewLifecycleOwner(), entries ->
        {
            mEntryAdapter = new DiaryAdapter(new ArrayList<DiaryEntry>(entries), this);
            mEntries.setAdapter(mEntryAdapter);
        });
        
        fab = root.findViewById(R.id.addDiary);
        fab.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                NavController nav = Navigation.findNavController(root);
                nav.navigate(R.id.action_diary_to_addDiaryEntry);
            }
        });
        return root;
    }
    
    @Override
    public void onListItemClick(int clickedItemIndex)
    {
        int entryNumber = clickedItemIndex + 1;
        Bundle bundle = new Bundle();
        DiaryEntry toEdit = DiaryViewModel.getAllEntries().getValue().get(clickedItemIndex);
        bundle.putSerializable("toEdit", toEdit);
        NavController nav = Navigation.findNavController(this.getView());
        nav.navigate(R.id.action_diary_to_addDiaryEntry, bundle);
    }
}