package via.sep4.Deprecated;

import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.lifecycle.ViewModelProviders;
import androidx.navigation.NavController;
import androidx.navigation.Navigation;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.text.InputType;
import android.view.View;
import android.view.ViewManager;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

import java.util.ArrayList;

import via.sep4.Model.Mushroom;

/**
 * @deprecated Superseded by Dashboard.
 */

public class DashboardActivity extends AppCompatActivity {

    /*//Old navigation bar - deprecated
    ImageButton buttonInfo;
    ImageButton buttonDashboard;
    ImageButton buttonSettings;


    ImageButton buttonAddMushroom;
    TableLayout tableLayout;
    TableRow row1;

    ArrayList<String> mushrooms = new ArrayList();
    ArrayList<Integer> tableRowIds = new ArrayList();

    DashboardViewModel dashboardViewModel;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.fragment_dashboard);
        buttonAddMushroom = (ImageButton) findViewById(R.id.btnAddMushroom);
        row1 = (TableRow) findViewById(R.id.dashboardTableRow1);
        tableLayout = (TableLayout) findViewById(R.id.dashboardTable);

        dashboardViewModel = ViewModelProviders.of(this).get(DashboardViewModel.class);
        dashboardViewModel.setData(getApplicationContext(), row1, (ImageButton) findViewById(R.id.mushroomButton), getDrawable(R.drawable.shroom), (TextView) findViewById(R.id.mushroomText), (LinearLayout) findViewById(R.id.containerMushroom));
        dashboardViewModel.addMushroom(new Mushroom("Latticed Stinkhorn"));
        dashboardViewModel.addMushroom(new Mushroom("Treehugger"));
        dashboardViewModel.addMushroom(new Mushroom("Puffball"));
        dashboardViewModel.addMushroom(new Mushroom("Indigo Milkcap"));
        buttonAddMushroom.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                openAddMushroomDialog(v);
            }
        });
        //dashboardActivityViewModel.reSetUpGrid() //Only for testing;
        UpdateGrid();
    }

    public void UpdateGrid() {
        tableLayout.removeAllViews();
        for (TableRow row: dashboardViewModel.getGrid()) {
            tableLayout.addView(row);
        }
    }

    public void NavigateDashboard(View view) {
        Intent intentActivityAddMushroom = new Intent(DashboardActivity.this, MainActivity.class);
        startActivity(intentActivityAddMushroom);
    }

    public void NavigateGeneralInfo(View view) {
        Intent intentActivityAddMushroom = new Intent(DashboardActivity.this, MainActivity.class);
        startActivity(intentActivityAddMushroom);
    }

    public void NavigateSettings(View view) {
        Intent intentActivityAddMushroom = new Intent(DashboardActivity.this, MainActivity.class);
        startActivity(intentActivityAddMushroom);
    }


    //On clicking + this brings a dialog with input
    public void AddMushroom(String mushroomName) {
        dashboardViewModel.addMushroom(new Mushroom(mushroomName));
        UpdateGrid();
    }

    public void AddMushroom(View view) {
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle("Mushroom Name");

        final EditText input = new EditText(this);
        input.setInputType(InputType.TYPE_CLASS_TEXT | InputType.TYPE_TEXT_VARIATION_PERSON_NAME);
        builder.setView(input);

        builder.setPositiveButton("Add Mushroom", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dashboardViewModel.addMushroom(new Mushroom(input.getText().toString()));
                UpdateGrid();
            }
        });
        builder.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.cancel();
            }
        });

        builder.show();
    }

    public void RemoveMushroom(LinearLayout containerToRemove) {
        int removalInThisRow = ((View) containerToRemove.getParent()).getId();
        int rowsToUpdate = tableRowIds.size() - tableRowIds.indexOf(removalInThisRow);
        for (int i = rowsToUpdate; i < tableRowIds.size(); i++) {
            TableRow rowToUpdate = (TableRow) findViewById(tableRowIds.get(i));
        }
        ((ViewManager) containerToRemove.getParent()).removeView(containerToRemove);

    }

    public void openAddMushroomDialog(View v) {
        AddMushroomDialogFragment dialogFragment = new AddMushroomDialogFragment();
        dialogFragment.show(getSupportFragmentManager(), "AddMushroomDialogFragment");
    }*/
}