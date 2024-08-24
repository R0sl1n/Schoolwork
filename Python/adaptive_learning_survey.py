"""
School assignment where we were collecting data from over 100 teatcher regarding the use of adaptive learning tools in math education in middleschool.
Written by Lill-Kristin Karlsen i Oct/Nov 2023.
"""
import pandas as pd
import seaborn as sns
import matplotlib.pyplot as plt
from sklearn.preprocessing import OrdinalEncoder
import matplotlib.patches as mpatches
import textwrap
import re
from fuzzywuzzy import fuzz

# 1. File preperation.
file_path = "Undervisningsmetoder_nyeste.xlsx"
df = pd.read_excel(file_path)
df_copy = df

# Given columnname.
columns_given = [
    "Kjønn",
    "Alder",
    "Hvilket fylke underviser du i?",
    "Hvor mange års pedagogisk utdanning har du?",
    "Hva slags formell utdanning har du i matematikk?",
    "Hvordan ville du rangert din tekniske kompetanse når det kommer til IT-utstyr og programvare? ",
    "Hvilket trinn underviser du matematikk på i år? (ta utgangspunkt i det trinnet du har flest undervisningsøkter i)",
    "Hvilke undervisningformer mener du fremmer elevenes kunnskap i matematikkfaget? Velg de tre du mener er viktigst. ",
    "Valg av læreverk til faget du underviser i ble tatt på bakgrunn av..?",
    "Burde valget av adaptive digitale læringsplattformer for matematikkfaget være en nasjonal beslutning, støttet av øremerkede midler, for å sikre en enhetlig tilnærming til dette i norske skoler? (g...",
    "Hvor ofte bruker du digitale verktøy i undervisningen din? (herunder også digitale verktøy som ikke er adaptive.)",
    "Hvordan mener du at digitale læringsplattformer påvirker elevenes læring?",
    "Har du tidligere erfaring med adaptive læringsverktøy i matematikk?",
    "Dersom ja på spørsmål 13 - Hvilket av følgende læringsverktøy har du benyttet?",
    "Hvor enig er du i denne påstanden: \"Adaptive læringsverktøy kan forbedre elevens forståelse i matematikk.\"",
    "Hvilke utfordringer eller bekymringer har du knyttet til bruk av adaptive læringsverktøy i matematikkundervisningen?",
    "Hvilke fordeler mener du det er med adaptive digitale læringsplattformer?",
    "Ville du vært åpen for å delta i en videre opplæring eller workshop om adaptive læringsverktøy i matematikk?",
    "Burde det vært satt av mer tid til intern opplæring i bruk av adaptive læringsverktøy på arbeidsplassen?",
    "Har du tilstrekkelig tid og ressurser til å gi individuell oppfølging til hver av dine elever? "
]
# Defining options in each category.
categories = [
    ["Kvinne", "Mann", "Ikke-binær"], # 1. Kjønn
    ["20-30", "31-40", "41-50", "51-60", "61-70", "Annet"], # 2. Alder
    ["Troms og Finnmark", "Nordland", "Møre og Romsdal", "Trøndelag", "Innlandet", "Vestland", "Rogaland", "Agder", "Vestfold og Telemark", "Oslo", "Viken"], # 3. Fylke
    ["0-2", "3-4", "5-6", "mer enn 6 år"], # 4. Pedagogisk utdanning
    ["Bachelorgrad i matematikk eller matematikkdidaktikk", "Mastergrad i matematikk eller matematikkdidaktikk", "Ingen spesifikk matematikkutdanning, men pedagogisk bakgrunn", "4-årig lærerutdanning med minimum 30 stp i matematikk", "60 stp + i matematikk i tillegg til praktisk pedagogisk utdanning", "Annet"], # 5. Utdanning i matematikk
    ["Høy", "Middels", "Lav"], # 6. Teknisk kompetanse
    ["8. klasse", "9. klasse", "10. klasse"], # 7. Trinn undervist
    ["Tradisjonell klasseromsundervisning hvor lærer gjennomgår pensum og elevene jobber med bestemte oppgaver", "Stasjonsarbeid hvor elevene møter på ulike utfordringer på hver post og kommer fram til svar i fellesskap", "Selvstendig arbeid, ettersom faget krever mengdetrening", "Pugging av grunnleggende basisferdigheter som gangetabellen og andre relevante regler innen matematikkfaget", "Digitale oppgaver som er såkalt 'adaptive' og tilpasses ut i fra hva eleven svarer. Eksempelvis Multi Smart Øving etc."], # 8. Undervisningsformer
    ["Økonomi", "Skolen har forhåndsbestemt forlag og kjøpt inn bøker", "Læreplattformen/boka tillater enkel differensiering til ulike elever", "Fremstillingen av bøkene og min egen opplevelse av hvordan læringsplattformen er utformet"], # 9. Valg av læreverk
    ["Ja, absolutt.", "Nøytral", "Uenig, dette ville begrenset skolene."], # 10. Nasjonal beslutning
    ["Daglig", "Ukentlig", "Månedlig", "Sjelden eller aldri"], # 11. Bruk av digitale verktøy
    ["De lærer like mye som ved annen variert undervisning", "Det øker motivasjonen og elevene lærer derfor mer", "De bidrar til at hver elev får tettet kunnskapshull i høyere grad, og øker elevenes kunnskapsnivå", "De distraherer elevene mer enn de bidrar til læring"], # 12. Påvirkning av digitale plattformer
    ["Ja", "Nei", "Vet ikke"], # 13. Erfaring med adaptive verktøy
    ["Multi Smart Øving", "Khan Academy", "DreamBox", "Annet"], # 14. Benyttede verktøy
    # 15 is an open-ended question with varying answers
    ["Enig", "Nøytral", "Uenig", "Har for dårlig erfaringsgrunnlag til å vurdere"], # 16. Meninger om adaptive verktøy
    ["Tekniske utfordringer (mangel på chromebooks, pcer, ipader, nettverksproblemer m.m)", "Kjenner for dårlig til hvordan å utnytte dette fullt ut", "Får mindre tid til personlig oppfølging av elevene", "Innsamling og behandling av persondata", "Skeptisk til å la algoritmer være en tungtveiende faktor for vurdering", "Fordelene ved å bruke dette overskygger eventuelle utfordringer"], # 17. Utfordringer med verktøy
    ["Enklere differensiering til hver enkelt elev", "Mer motiverte elever", "Mer tid til hver elev, da jeg slipper å bruke tiden på tilpasse oppgavene (siden læringsplattformen gjør dette)", "Målbare resultater over tid som gir godt sammenligningsgrunnlag", "Statistikk fra de adaptive læringsplattformene vil gi meg bedre totaloversikt over elevenes behov", "Utfordringene ved å bruke dette overskygger eventuelle fordeler"], # 18. Fordeler med plattformer
    ["Ja, definitivt", "Kanskje, avhenger av innhold", "Nei, ser ikke at dette vil kunne være et fullgodt alternativ i undervisningen"], # 19. Åpenhet for opplæring
    ["Ja, absolutt. Dette trenger vi å lære mer om", "Jeg får satt av noe tid til digital videreutvikling, men ikke nok innenfor dette temaet", "Nei, dette er ikke noe vi ønsker å prioritere"], # 20. Intern opplæring
    ["Ja, jeg føler at jeg har tilstrekkelig tid og ressurser", "Ja, tiden strekker til, men adaptive læringsverktøy kan gi enda bedre oppfølging", "Nei, tid og ressurser strekker ikke til, og adaptive verktøy kan være nyttige i den sammenheng", "Nei, tid og ressurser strekker ikke til, men jeg har ikke tillit til at adaptive læringsverktøy er løsningen"] # 21. Tid og ressurser for oppfølging
]

# Splitting multiple choice questions.
for col in df.columns:
    if df[col].dtype == 'O':  # Check if column data type is 'object' (usually string in pandas)
        df[col] = df[col].str.rstrip(';')

# Filter dataframe to only include given columns.
df = df[columns_given]

# Multi-choice columns.
multi_choice_columns = [
    "Hvilke undervisningformer mener du fremmer elevenes kunnskap i matematikkfaget? Velg de tre du mener er viktigst. ",
    "Valg av læreverk til faget du underviser i ble tatt på bakgrunn av..?",
    "Dersom ja på spørsmål 13 - Hvilket av følgende læringsverktøy har du benyttet?",
    "Hvilke utfordringer eller bekymringer har du knyttet til bruk av adaptive læringsverktøy i matematikkundervisningen?",
    "Hvilke fordeler mener du det er med adaptive digitale læringsplattformer?"
]

# Filter copy of dataframe to only include input answers from question 15.
df_copy = df_copy['Hvis du svarte "Annet" på spørsmål 14, skriv gjerne her hvilke adaptive læringsverktøy du har brukt. ']
df_copy = df_copy.str.replace('\xa0', '').dropna()

unique_answers = set()
for answers in df_copy:
    split_answers = [answer.strip().lower() for answer in re.split(',| og ', str(answers))]
    unique_answers.update(split_answers)

# Remove empty strings if any
unique_answers.discard('')
print(unique_answers)

threshold = 54
processed_set = set()
duplicates = {}
original_answer_counts = {}

# Count all entries
for answers in df_copy:
    split_answers = [answer.strip().lower() for answer in re.split(',| og ', str(answers))]
    for answer in split_answers:
        if answer:
            original_answer_counts[answer] = original_answer_counts.get(answer, 0) + 1

# Identify duplicates and assign them to original answer
for answer in unique_answers:
    if answer not in processed_set:
        processed_set.add(answer)
        for potential_duplicate in unique_answers:
            if potential_duplicate != answer and potential_duplicate not in duplicates:
                if fuzz.ratio(answer, potential_duplicate) > threshold:
                    duplicates[potential_duplicate] = answer

# Update processed_set to remove duplicates
for duplicate, original in duplicates.items():
    if duplicate in original_answer_counts:
        # Add the count value from duplicate to original answer, already present in processed_set
        original_answer_counts.setdefault(original, 0)  # Sikrer at original har en verdi
        original_answer_counts[original] += original_answer_counts.pop(duplicate)

# Handling special cases.
if 'kikora. skolenmin.cdu.no' in original_answer_counts:
    original_answer_counts.setdefault('skolenmin.cdu.no', 0)  # Sikrer at 'skolenmin.cdu.no' har en verdi
    original_answer_counts['skolenmin.cdu.no'] += original_answer_counts.pop('kikora. skolenmin.cdu.no')
    processed_set.discard('kikora. skolenmin.cdu.no')
    processed_set.add('skolenmin.cdu.no')

processed_set = {key for key in processed_set if key in original_answer_counts}
# Correct counter value
sorted_processed_answer_counts = {k: original_answer_counts[k] for k in sorted(processed_set)}

#Manually adjusting a value due to threshold complications.
sorted_processed_answer_counts.pop('maximum')
sorted_processed_answer_counts['maximum smart øving'] = 3

# Max length for each category label.
MAX_LABEL_LENGTH = 30
# Adjusted font sizes
PIE_LABEL_FONT_SIZE = 8
TITLE_FONT_SIZE = 10

# Function to truncate and add ellipsis to long labels.
def truncate_label(label):
    if len(label) > MAX_LABEL_LENGTH:
        return label[:MAX_LABEL_LENGTH] + "..."
    else:
        return label

# Function to wrap label text across multiple lines if too long.
def wrap_label(label, max_length=MAX_LABEL_LENGTH):
    # Use textwrap to wrap text at the specified max_length, without splitting words
    wrapped_lines = textwrap.wrap(label, max_length)
    # Join the wrapped lines with a newline character to create a multi-line string
    return '\n'.join(wrapped_lines)
    
# Function to split long labels into two lines.
def split_title(title):
    if len(title) <= MAX_LABEL_LENGTH:
        return title
    else:
        half_len = len(title) // 2
        # Find nearest space to split title
        split_point = title.rfind(' ', 0, half_len)
        return title[:split_point] + "\n" + title[split_point:].lstrip()

# Initializing the plot_counter.    
plot_counter = 1

# Setting colorscheme.
colors = plt.colormaps['tab20'](range(len(categories)))

# # Plot data
for column in df.columns:
    #Defining filename from plot_counter value.
    filename = f"plot_{plot_counter}.png"
    plot_counter += 1
    
    # Check if column is multiple choice question
    if column in multi_choice_columns:
        # Split the column based on ';'
        split_data = df[column].str.split(';', expand=True).stack().value_counts()
        # Sort alphabetically, but 'Annet' last
        sorted_index = sorted(split_data.index, key=lambda x: (x == "Annet", x))
        split_data = split_data.reindex(sorted_index)
        plt.bar(range(len(sorted_index)), split_data.values, color=colors[:len(sorted_index)])
        plt.title(split_title(column), fontsize=TITLE_FONT_SIZE)
        plt.ylabel('Antall')
        plt.xlabel('')

        # Create a legend with wrapped labels
        legend_labels = [wrap_label(label) for label in sorted_index]
        legend_patches = [mpatches.Patch(color=color, label=label) for color, label in zip(colors, legend_labels)]
        legend = plt.legend(handles=legend_patches, loc='center left', bbox_to_anchor=(1, 0.60), fontsize=PIE_LABEL_FONT_SIZE)
        
        plt.xticks([])  # Remove x-tick labels
        
        plt.tight_layout()
        plt.savefig(filename, dpi=300, bbox_extra_artists=(legend,), bbox_inches='tight')
        plt.show()
    else:
        column_data = df[column].value_counts()
        sorted_index = sorted(column_data.index, key=lambda x: (x == "Annet", x))
        column_data = column_data.reindex(sorted_index)
        
        # Generate pie chart for categories with 5 or fewer answer options
        if len(categories[columns_given.index(column)]) <= 5:
            labels_need_truncating = any(len(label) > MAX_LABEL_LENGTH for label in sorted_index)
            if labels_need_truncating:
                # Draw the pie chart without labels
                plt.pie(column_data, autopct='%1.1f%%', colors=colors[:len(sorted_index)], textprops={'fontsize': PIE_LABEL_FONT_SIZE})
                plt.title(split_title(column), fontsize=TITLE_FONT_SIZE)
                
                # Create a legend with wrapped labels
                legend_labels = [wrap_label(label) for label in sorted_index]
                legend_patches = [mpatches.Patch(color=color, label=label) for color, label in zip(colors, legend_labels)]
                legend = plt.legend(handles=legend_patches, loc='center left', bbox_to_anchor=(0.9, 0.8), fontsize=PIE_LABEL_FONT_SIZE)
                plt.tight_layout()
                plt.savefig(filename, dpi=300, bbox_extra_artists=(legend,), bbox_inches='tight')
                plt.show()
            else:
                # Draw the pie chart with labels
                plt.pie(column_data, labels=[truncate_label(label) for label in sorted_index], autopct='%1.1f%%', colors=colors[:len(sorted_index)], textprops={'fontsize': PIE_LABEL_FONT_SIZE})
                plt.title(split_title(column), fontsize=TITLE_FONT_SIZE)
                
                plt.savefig(filename, dpi=300)
                plt.show()
        else:
            # Generate bar chart for categories with more than 5 answer options
            labels_need_truncating = any(len(label) > MAX_LABEL_LENGTH for label in sorted_index)
            
            if labels_need_truncating:
                plt.bar(range(len(sorted_index)), column_data.values, color=colors[:len(sorted_index)])
                plt.title(split_title(column), fontsize=TITLE_FONT_SIZE)
                plt.ylabel('Antall')
                plt.xlabel('')
                plt.xticks(range(len(sorted_index)), [''] * len(sorted_index))  # Remove x-tick labels
                
                legend_labels = [wrap_label(label) for label in sorted_index]
                legend_patches = [mpatches.Patch(color=color, label=label) for color, label in zip(colors, legend_labels)]
                legend = plt.legend(handles=legend_patches, loc='center left', bbox_to_anchor=(1, 0.60), fontsize=PIE_LABEL_FONT_SIZE)
                
                plt.tight_layout()
                plt.savefig(filename, dpi=300, bbox_extra_artists=(legend,), bbox_inches='tight')
                plt.show()
            else:
                plt.bar(range(len(sorted_index)), column_data.values, color=colors[:len(sorted_index)])
                plt.title(split_title(column), fontsize=TITLE_FONT_SIZE)
                plt.ylabel('Antall')
                plt.xlabel('')
                plt.xticks(range(len(sorted_index)), [truncate_label(label) for label in sorted_index], rotation=45, ha='right', fontsize=PIE_LABEL_FONT_SIZE)
                
                plt.tight_layout()
                plt.savefig(filename, dpi=300)
                plt.show()

           
# Create a plot for the sorted_processed_answer_counts
def plot_sorted_processed_answer_counts(sorted_processed_answer_counts, plot_counter):
    plt.figure(figsize=(10, 8))

    # Generate color scheme
    color_scheme = plt.cm.tab20(range(len(sorted_processed_answer_counts)))

    # Create sorted lists of keys and values
    sorted_keys = sorted(sorted_processed_answer_counts, key=sorted_processed_answer_counts.get, reverse=True)
    sorted_values = [sorted_processed_answer_counts[key] for key in sorted_keys]

    # Create bar plot
    plt.bar(range(len(sorted_keys)), sorted_values, color=color_scheme)

    # Set title and labels
    plt.title('Lærernes egendefinerte adaptive verktøy i spørsmål 15', fontsize=TITLE_FONT_SIZE)
    plt.xlabel('Verktøy', fontsize=PIE_LABEL_FONT_SIZE)
    plt.ylabel('Antall', fontsize=PIE_LABEL_FONT_SIZE)

    # Remove x-tick labels
    plt.xticks([])  # This will remove the x-tick labels
    
    # Create legend
    legend_labels = [wrap_label(key) for key in sorted_keys]
    legend_patches = [mpatches.Patch(color=color, label=label) for color, label in zip(color_scheme, legend_labels)]
    plt.legend(handles=legend_patches, loc='center left', bbox_to_anchor=(1, 0.5), fontsize=PIE_LABEL_FONT_SIZE)

    # Layout adjustment and plot saving
    plt.tight_layout()
    plt.savefig(f"plot_{plot_counter}.png", dpi=300, bbox_inches='tight')
    plt.show()

# Assuming sorted_processed_answer_counts is a dictionary containing the data to plot
plot_counter = 50  # Initialize plot_counter if it's not already defined
plot_sorted_processed_answer_counts(sorted_processed_answer_counts, plot_counter)


# Removing special char \xa0 from columnname and values.
df.columns = [col.replace('\xa0', '') for col in df.columns]
df = df.map(lambda x: x.replace('\xa0', '') if isinstance(x, str) else x)

#OrdinalEncoder on the entire dataframe to convert character answers to numeric.
encoder = OrdinalEncoder()
df_encoded = encoder.fit_transform(df)
transformed_df = pd.DataFrame(df_encoded, columns=df.columns, index=df.index)
transformed_df.fillna(transformed_df.median(), inplace=True)

# Multi-choice columns - In order to correctly identify the columns, now without special chars, the variable is defined again.
multi_choice_columns = [
    "Hvilke undervisningformer mener du fremmer elevenes kunnskap i matematikkfaget? Velg de tre du mener er viktigst.",
    "Valg av læreverk til faget du underviser i ble tatt på bakgrunn av..?",
    "Dersom ja på spørsmål 13 - Hvilket av følgende læringsverktøy har du benyttet?",
    "Hvilke utfordringer eller bekymringer har du knyttet til bruk av adaptive læringsverktøy i matematikkundervisningen?",
    "Hvilke fordeler mener du det er med adaptive digitale læringsplattformer?"
]

# Drop multi-choice columns from the DataFrame
df_filtered = transformed_df.drop(columns=multi_choice_columns, errors='ignore')  # The errors='ignore' ensures that the code doesn't break if a column is not in the dataFrame.

# Compute the correlations
correlations = df_filtered.corr()
# Plotting the heatmap
plt.figure(figsize=(10, 8))
sns.set(font_scale=0.7)  # Reduce fontsize
sns.heatmap(correlations, cmap='coolwarm', annot=True, fmt=".2f", linewidths=0.5, annot_kws={'fontsize': 8}, cbar_kws={'shrink': 0.75},cbar=True, xticklabels=[truncate_label(label) for label in correlations.columns],
            yticklabels=[truncate_label(label) for label in correlations.columns])
plt.title('Korrelasjonsheatmap')
plt.tight_layout()
plt.savefig(fname="heat.png", dpi=300)
plt.show()



