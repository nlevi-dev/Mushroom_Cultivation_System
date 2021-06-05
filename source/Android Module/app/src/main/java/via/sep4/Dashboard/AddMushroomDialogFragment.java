
package via.sep4.Dashboard;

import android.app.AlertDialog;
import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.EditText;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatDialogFragment;

import via.sep4.Dashboard.Dashboard;
import via.sep4.R;

public class AddMushroomDialogFragment extends AppCompatDialogFragment {
    private EditText editTextMushroomName;
    private AddMushroomDialogListener listener;
    private Dashboard dashboard;

    public AddMushroomDialogFragment(Dashboard dash)
    {
        dashboard = dash;
    }

    @NonNull
    @Override
    public Dialog onCreateDialog(@Nullable Bundle savedInstanceState) {
        AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
        LayoutInflater inflater = getActivity().getLayoutInflater();

        View v = inflater.inflate(R.layout.fragment_dialog_addmushroom, null);
        builder.setView(v).setTitle("AddMushroom").setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int i) {
                //Closes dialog
            }
        }).setPositiveButton("Add Mushroom", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int i) {
                //Gets Inputs and sends them to listener
                String mushroomName = editTextMushroomName.getText().toString();
                dashboard.AddMushroom(mushroomName);
            }
        });
        editTextMushroomName = v.findViewById(R.id.editTextMushroomName);
        return builder.create();
    }

    //Adds listener to pass data
    @Override
    public void onAttach(@NonNull Context context) {
        super.onAttach(context);
        try {
            listener = (AddMushroomDialogListener) context;
        } catch (ClassCastException e) {
            throw new ClassCastException(context.toString() + " Must implement AddMushroomDialog Listener");
        }

    }

    //The listener interface
    public interface AddMushroomDialogListener {
        //void applyData(String mushroomName, Dashboard dash);
    }
}
