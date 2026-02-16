import { Dialog, DialogContent, DialogFooter, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { Button } from "./ui/button";


export default function DeleteWarning({ open, onOpenChange, title, description, onDelete }: { open: boolean; onOpenChange: (open: boolean) => void; title: string; description: string; onDelete: () => void }) {
    return (
        <Dialog open={open} onOpenChange={onOpenChange}>
            <DialogContent>
                <DialogHeader>
                    <DialogTitle className="text-red-500">{title}</DialogTitle>
                </DialogHeader>
                <div className="mt-4">
                    <p>{description}</p>
                </div>
                <DialogFooter>
                    <Button variant="outline" onClick={() => {
                        onDelete();
                        onOpenChange(false);
                    }}>
                        حذف
                    </Button>
                    <Button variant="destructive" onClick={() => onOpenChange(false)}>إلغاء</Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
}