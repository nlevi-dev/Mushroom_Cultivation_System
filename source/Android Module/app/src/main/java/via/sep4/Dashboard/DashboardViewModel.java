package via.sep4.Dashboard;

import android.content.Context;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.util.Log;
import android.util.TypedValue;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TableRow;
import android.widget.TextView;

import androidx.core.view.ViewCompat;
import androidx.lifecycle.LifecycleOwner;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import via.sep4.Model.Specimen;
import via.sep4.Model.Mushroom;
import via.sep4.Persistence.WebClient;
import via.sep4.R;

public class DashboardViewModel extends ViewModel {
    //List containing all mushrooms
    private MutableLiveData<List<Mushroom>> mList;
    //List containing all generated rows
    private MutableLiveData<List<TableRow>> rList = new MutableLiveData<>();
    //Variables used to create Mushroom
    private Context context;
    private TableRow tableRow;
    private ImageButton shroomButton;
    private Drawable shroomButtonImage;
    private TextView shroomTextView;
    private LinearLayout shroomContainer;
    private List<Specimen> specimens;
    
    //Sets variables used to create Mushroom, generates Grid
    public void setData(Context context, TableRow tableRow, ImageButton shroomButton, Drawable shroomButtonImage, TextView shroomText, LinearLayout shroomContainer, LifecycleOwner owner) {
        this.context = context;
        this.tableRow = tableRow;
        this.shroomButton = shroomButton;
        this.shroomButtonImage = shroomButtonImage;
        this.shroomTextView = shroomText;
        this.shroomContainer = shroomContainer;
        mList = new MutableLiveData<>();
        List<Mushroom> mushroomList = new ArrayList<>();
        mList.setValue(mushroomList);
//        WebClient.getSpecimenAPI().getSpecimens().enqueue(new Callback<ResponseBody>()
//        {
//            @Override
//            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response)
//            {
//
//            }
//
//            @Override
//            public void onFailure(Call<ResponseBody> call, Throwable t)
//            {
//                System.out.println("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFAILURE");
//                System.out.println(t.getMessage());
//            }
//        });
        specimens = new ArrayList<>();
        WebClient.getSpecimenAPI().getSpecimens().enqueue(new Callback<List<Specimen>>()
        {
            @Override
            public void onResponse(Call<List<Specimen>> call, Response<List<Specimen>> response)
            {
                if(response.body() != null)
                {
                    specimens.addAll(response.body());
                    for (Specimen specimen : specimens)
                    {
                        Mushroom temp = new Mushroom(specimen.getSpecimen_name());
                        temp.setSpecimen_id(specimen.getSpecimen_key());
                        addMushroom(temp);
                    }
                        //mList.getValue().add(new Mushroom(specimen.getSpecimen_name()));
                }
            }

            @Override
            public void onFailure(Call<List<Specimen>> call, Throwable t)
            {
                System.out.println("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFAILURE");
                System.out.println(t.getMessage());
            }
        });
        
        setUpGrid();
    }

    //Adds a mushroom using its object
    public void addMushroom(Mushroom mushroom) {
        LinearLayout newShroomContainer = createNewMushroomContainer(mushroom);
        int numberOfRowsNeeded = ((getMushroomList().getValue().size() + 1) / 3);
        Log.i("INFO", mushroom.getName());
        Log.i("INFO", getMushroomList().getValue().size() + "");
        Log.i("INFO", "Current Grid Contains: " + getGrid().size());
        if (getMushroomList().getValue().size() % 3 != 0) {
            TableRow rowToAdd = getGrid().get(getGrid().size() - 1);
            rowToAdd.addView(newShroomContainer);
            mushroom.setParent(getGrid().get(getGrid().size() - 1));
            mushroom.setContainer(newShroomContainer);
            List<Mushroom> list = mList.getValue();
            list.add(mushroom);
            mList.setValue(list);
        } else {
            TableRow newRow = new TableRow(context);
            newRow.setId(ViewCompat.generateViewId());
            ViewGroup.LayoutParams tableRowLayout = tableRow.getLayoutParams();
            newRow.setLayoutParams(tableRowLayout);
            newRow.addView(newShroomContainer);
            mushroom.setParent(newRow);
            mushroom.setContainer(newShroomContainer);
            getGrid().add(newRow);
            getMushroomList().getValue().add(mushroom);
        }
    }

    //Remove mushroom using its object
    public void delMushroom(Mushroom mushroom) {
        getMushroomList().getValue().remove(mushroom);
        reSetUpGrid();
    }

    public List<TableRow> getGrid() {
        return rList.getValue();
    }

    //Possible shifting mechanism
    private void moveMushroomsLeft(int rowStart) {
        int mushroomFirst = rowStart * 3 - 3;
        Mushroom mushroom1 = null;
        Mushroom mushroom2 = null;
        Mushroom mushroom3 = null;
        int interval = 0;
        for (Mushroom mushroom :
                getMushroomList().getValue()) {
            if (mushroomFirst <= getMushroomList().getValue().indexOf(mushroom)) {
                if (interval <= 2) {
                    interval++;
                    switch (interval) {
                        case 1:
                            mushroom1 = mushroom;
                        case 2:
                            mushroom2 = mushroom;
                        case 3:
                            mushroom3 = mushroom;
                    }
                } else {
                    TableRow update = getGrid().get(getGrid().indexOf(mushroom1.getParent()));
                    update.removeAllViews();
                    if (mushroom1 != null) {
                        update.addView(mushroom1.getContainer());
                        mushroom1.setParent(update);
                    }
                    if (mushroom2 != null) {
                        update.addView(mushroom2.getContainer());
                        mushroom2.setParent(update);
                    }
                    if (mushroom3 != null) {
                        update.addView(mushroom3.getContainer());
                        mushroom3.setParent(update);
                    }
                    mushroom1 = mushroom2 = mushroom3 = null;
                    interval = 1;
                    mushroom1 = mushroom;
                }
            }
        }
    }

    //Call this as many times as you want
    public void reSetUpGrid() {
        int shrooms = 0;
        List<TableRow> newList = new ArrayList<>();
        rList.setValue(newList);
        for (Mushroom mushroom : getMushroomList().getValue()) {
            LinearLayout newShroomContainer;
            if (mushroom.getContainer() == null) {
                newShroomContainer = createNewMushroomContainer(mushroom);
            } else {
                newShroomContainer = mushroom.getContainer();
            }
            //Assign this mushroom to a row(existing or new)
            if (shrooms % 3 == 0) {
                TableRow newRow = new TableRow(context);
                newRow.setId(ViewCompat.generateViewId());
                ViewGroup.LayoutParams tableRowLayout = tableRow.getLayoutParams();
                newRow.setLayoutParams(tableRowLayout);
                newRow.addView(newShroomContainer);
                mushroom.setParent(newRow);
                mushroom.setContainer(newShroomContainer);
                getGrid().add(newRow);
            } else {
                TableRow rowToAdd = getGrid().get((int) shrooms / 3);
                mushroom.setParent(rowToAdd);
                mushroom.setContainer(newShroomContainer);
                rowToAdd.addView(newShroomContainer);
            }
            //Increase mushrooms size for row generation
            shrooms++;
        }
    }

    //Call this only once for setup
    private void setUpGrid() {
        int shrooms = 0;
        List<TableRow> newList = new ArrayList<>();
        rList.setValue(newList);
        for (Mushroom mushroom : getMushroomList().getValue()) {
            LinearLayout newShroomContainer = createNewMushroomContainer(mushroom);
            //Assign this mushroom to a row(existing or new)
            if (shrooms % 3 == 0) {
                TableRow newRow = new TableRow(context);
                newRow.setId(ViewCompat.generateViewId());
                ViewGroup.LayoutParams tableRowLayout = tableRow.getLayoutParams();
                newRow.setLayoutParams(tableRowLayout);
                newRow.addView(newShroomContainer);
                mushroom.setParent(newRow);
                mushroom.setContainer(newShroomContainer);
                rList.getValue().add(newRow);
            } else {
                TableRow rowToAdd = getGrid().get((int) shrooms / 3);
                mushroom.setParent(rowToAdd);
                mushroom.setContainer(newShroomContainer);
                rowToAdd.addView(newShroomContainer);
            }
            //Increase mushrooms size for row generation
            shrooms++;
        }
    }

    //Used to create a container for mushroom
    private LinearLayout createNewMushroomContainer(Mushroom mushroom) {
        //NavController nav = new NavController(context); //Need view object here, not accessible in viewmodel :(
        String mushroomName = mushroom.getName();
        //ImageButton for new shroom
        ImageButton newShroomButton = new ImageButton(context);
        ViewGroup.LayoutParams layoutShroomButton = shroomButton.getLayoutParams();
        newShroomButton.setLayoutParams(layoutShroomButton);
        newShroomButton.setMinimumWidth(toDensity(context, 97));
        newShroomButton.setMinimumHeight(toDensity(context, 92));
        newShroomButton.setBackgroundResource(R.drawable.shroom);
        newShroomButton.setBackground(shroomButtonImage);
        newShroomButton.setClickable(true);
        newShroomButton.setScaleType(ImageView.ScaleType.FIT_XY);
        newShroomButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Bundle bundle = new Bundle();
                bundle.putSerializable("mushroom", mushroom);
                NavController nav = Navigation.findNavController(v);
                nav.navigate(R.id.action_dashboard_to_viewSpecimen, bundle);
            }
        });


        //TextView for new shroom
        TextView newShroomText = new TextView(context);
        ViewGroup.LayoutParams layoutShroomText = shroomTextView.getLayoutParams();
        newShroomText.setLayoutParams(layoutShroomText);
        newShroomText.setMinEms(10);
        newShroomText.setText(mushroomName);
        newShroomText.setTextAlignment(View.TEXT_ALIGNMENT_CENTER);
        newShroomText.setTranslationZ(3);
        newShroomText.setTextColor(Color.BLACK);
        newShroomText.setId(ViewCompat.generateViewId());
        //LinearLayout
        LinearLayout newShroomContainer = new LinearLayout(context);
        newShroomContainer.setTag("shroom-" + mushroomName);
        ViewGroup.LayoutParams lp = shroomContainer.getLayoutParams();
        newShroomContainer.setLayoutParams(lp);
        newShroomContainer.setOrientation(LinearLayout.VERTICAL);
        newShroomContainer.setId(ViewCompat.generateViewId());
        newShroomContainer.addView(newShroomButton);
        newShroomContainer.addView(newShroomText);
        return newShroomContainer;
    }

    //Returns a list of Mushrooms or generates a new one
    public MutableLiveData<List<Mushroom>> getMushroomList() {
        if (mList == null) {
            mList = new MutableLiveData<>();
            loadMushroomList();
        }
        return mList;
    }

    //Generates a new Mushroom List
    private void loadMushroomList() {
        List<Mushroom> mushroomListExample = new ArrayList<>();
        mushroomListExample.add(new Mushroom("Mushroom1"));
        mushroomListExample.add(new Mushroom("Mushroom2"));
        mushroomListExample.add(new Mushroom("Mushroom3"));
        mushroomListExample.add(new Mushroom("Mushroom4"));
        mushroomListExample.add(new Mushroom("Mushroom5"));
        mList.setValue(mushroomListExample);
    }

    //This is used to get density values instead of pixels
    private static int toDensity(final Context context, final float px) {
        return (int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, px, context.getResources().getDisplayMetrics());
    }
}